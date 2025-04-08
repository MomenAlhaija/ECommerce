using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Models;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Customer";
    public CreateCustomerHandler(IMapper mapper, ICustomerRepository customerRepository, ILogger<CreateCustomerHandler> logger, IDistributedCache cache)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = _mapper.Map<Customer>(request);

            Customer customerFromDb= await _customerRepository.AddAsync(customer);
            _logger.LogInformation("Added Customer Name :{0} with Email {1} Successfully", request.Name, request.Email);
            
            string key = Prefix + customerFromDb.Id;
            string json = JsonConvert.SerializeObject(customerFromDb);
            await _cache.SetStringAsync(key, json);
            _logger.LogInformation("Add {0} into Cache", key);

            return _mapper.Map<CustomerDto>(customer);
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
