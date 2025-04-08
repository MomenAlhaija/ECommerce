using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace ECommerce.Application.Features;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, List<CustomerDto>>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCustomersQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Customers";
    public GetCustomersQueryHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<GetCustomersQueryHandler> logger, IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<CustomerDto>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<Customer> customers= await _customerRepository.ListAllAsync();
           
            if (customers is { Count: 0 })
            {
                _logger.LogError("can't load customers");
            }

            return _mapper.Map<List<CustomerDto>>(customers);
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
