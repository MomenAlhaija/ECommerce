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

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
{
    private readonly IMapper _mapper;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Order";
    public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository orderRepository, ILogger<UpdateOrderCommandHandler> logger, ICustomerRepository customerRepository, IDistributedCache cache)
    {
        _mapper = mapper;
        _orderRepository = orderRepository;
        _logger = logger;
        _customerRepository = customerRepository;
        _cache = cache;
    }

    public async Task<OrderDto> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = await _orderRepository.GetByIdAsync(request.Id);

            if (order is null)
            {
                _logger.LogError($"Not Foud Order With Id:{request.Id}");
                throw new NotFoundException($"Not Foud Order With Id:{request.Id}");
            }
            if (await _customerRepository.GetByIdAsync(request.CustomerId) is null)
            {
                _logger.LogError("Invalied Customer Assigned To Order");
                throw new NotFoundException("Invalied Customer Assigned To Order");
            }
            order.Payment = order.OrderItems.Sum(p => p.Price * p.Quantity);
            var orderToUpdate = _mapper.Map<Order>(request);

            await _orderRepository.UpdateOrderWithItems(orderToUpdate);
            _logger.LogInformation("Update Order Successfully {0}",order);

            string key = Prefix + request.Id;
            string json = JsonConvert.SerializeObject(orderToUpdate);
            await _cache.SetStringAsync(key, json);
            _logger.LogInformation("Add Key {0} into cache", key);


            return _mapper.Map<OrderDto>(orderToUpdate);
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }

    }
}
