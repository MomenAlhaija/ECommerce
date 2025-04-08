using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features;

public class DeleteCustomerHandler :IRequestHandler<DeleteCustomerCommand,bool>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCustomerHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Customer";
    public DeleteCustomerHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<DeleteCustomerHandler> logger, IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await _customerRepository.GetByIdAsync(request.Id);
            string key = Prefix + request.Id;

            if (customer is null)
            {
                _logger.LogError("Not Found Customer With Id:{0}", request.Id);
                throw new NotFoundException("Not Found Customer With Id:{0}", request.Id);
            }
            
            await _customerRepository.DeleteAsync(customer);
            _logger.LogInformation("Delete Customer with Id {0} Have Name {1} Successfully", request.Id, customer.Name);

            await _cache.RemoveAsync(key);
            _logger.LogInformation("Removed Key {0} from Cache", key);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
