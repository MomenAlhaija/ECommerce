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

public class GetCustomerDetailsQueryHandler : IRequestHandler<GetCustomerDetailsQuery, CustomerDto>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<GetCustomerDetailsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Customer";
    public GetCustomerDetailsQueryHandler(IMapper mapper, ICustomerRepository customerRepository, ILogger<GetCustomerDetailsQueryHandler> logger, IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CustomerDto> Handle(GetCustomerDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Customer customer;
            string key = Prefix + request.Id;
            if (await _cache.GetStringAsync(key)!=null)
            {
                string value = await _cache.GetStringAsync(key);
                customer = JsonConvert.DeserializeObject<Customer>(value);
            }
            else
            {
                customer = await _customerRepository.GetByIdAsync(request.Id);
                if (customer is null)
                {
                    _logger.LogError("Not Found Customer With Id:{0}", customer.Id);
                    throw new NotFoundException("Not Found Custoemr");
                }
                string json = JsonConvert.SerializeObject(customer);
                await _cache.SetStringAsync(key, json);
                _logger.LogInformation("add key {0} into cache", key);

            }
            return _mapper.Map<CustomerDto>(customer);
        }
        catch(Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
