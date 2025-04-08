using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features;

public class DeleteOrderCommandHandler :IRequestHandler<DeleteOrderCommand,bool>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Order";
    public DeleteOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<DeleteOrderCommandHandler> logger, IDistributedCache cache)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);
            if (order is null)
            {
                _logger.LogError("Not Found Order with ID:{0}", request.Id);
                throw new NotFoundException("Order Not Found");
            }
            await _orderRepository.DeleteOrderWithItems(order.Id);
            _logger.LogInformation("Delte Order have Id {0} Suessully",request.Id);

            string key = Prefix + request.Id;
            await _cache.RemoveAsync(key);
            _logger.LogInformation("Removed Key {0} from Cache",key);

            return true;
        }
        catch(Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }


}
