using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class GetOrderDetailsQueryHandler : IRequestHandler<GetOrderDetailsQuery, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrderDetailsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Order";
    public GetOrderDetailsQueryHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderDetailsQueryHandler> logger, IDistributedCache cache)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<OrderDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Order order;
            string key = Prefix + request.Id;
            if (await _cache.GetStringAsync(key)!=null)
            {
                string value = await _cache.GetStringAsync(key);
                order = JsonConvert.DeserializeObject<Order>(value);
                return _mapper.Map<OrderDto>(order);
            }
            else
            {
                order = await _orderRepository.GetByIdAsync(request.Id);
                if (order is null)
                {
                    _logger.LogError("Not Found Order With Id:{0}", request.Id);
                    throw new NotFoundException(String.Format("Not Found Order With Id:{0}", request.Id));
                }
                _logger.LogInformation("get Order by Id :{0}", request.Id);

                string json = JsonConvert.SerializeObject(order);
                await _cache.SetStringAsync(key, json);
                _logger.LogInformation("Add Key {0} into Cahe",key);

                return _mapper.Map<OrderDto>(order);
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
