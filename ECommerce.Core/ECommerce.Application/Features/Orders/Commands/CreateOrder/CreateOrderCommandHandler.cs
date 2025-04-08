using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Application.Features;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateOrderCommandHandler> _logger;
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Order";
    public CreateOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<CreateOrderCommandHandler> logger, ICustomerRepository customerRepository, IDistributedCache cache)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
        _customerRepository = customerRepository;
        _cache = cache;
    }

    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {

        try
        {
            if (request is null)
            {

                _logger.LogError("Request is invalied {0}", request);
                throw new BadRequestException("Invalied request Body");
            }
            if (await _customerRepository.GetByIdAsync(request.CustomerId) is null)
            {
                _logger.LogError("Invalied Customer Assigned To Order");
                throw new NotFoundException("Invalied Customer Assigned To Order");
            }
            var order = _mapper.Map<Order>(request);

            order.Payment = order.OrderItems.Sum(p => p.Price * p.Quantity);

            var orderFromDb = await _orderRepository.AddOrderWithItems(order);
            _logger.LogInformation("Order Create Suessfully {0}", orderFromDb);

            string key = Prefix + orderFromDb.Id;
            string json = JsonConvert.SerializeObject(orderFromDb);
            await _cache.SetStringAsync(key, json);
            _logger.LogInformation("Add Key {0} into cache", key);



            return _mapper.Map<OrderDto>(orderFromDb);
        }
        catch (Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
