
using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Numerics;

namespace ECommerce.Application.Features;

public class GetOrdersQueyHandler : IRequestHandler<GetOrdersQuey, List<OrderDto>>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetOrderDetailsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Orders";
    public GetOrdersQueyHandler(IOrderRepository orderRepository, IMapper mapper, ILogger<GetOrderDetailsQueryHandler> logger, IDistributedCache cache)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<OrderDto>> Handle(GetOrdersQuey request, CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<Order> orders= await _orderRepository.ListAllAsync();

            if (orders is { Count: 0 })
            {
                _logger.LogError("Can't Load Orders");
                return new List<OrderDto>();
            }

            _logger.LogInformation("Loaded Orders {0}", orders);

            return _mapper.Map<List<OrderDto>>(orders);
            
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;

        }

    }
}
