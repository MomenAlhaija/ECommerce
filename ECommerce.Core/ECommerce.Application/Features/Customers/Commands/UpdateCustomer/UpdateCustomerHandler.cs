using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCustomerHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Customer";
    public UpdateCustomerHandler(ICustomerRepository customerRepository, IMapper mapper, ILogger<UpdateCustomerHandler> logger, IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var customerIsExist = await _customerRepository.GetByIdAsync(request.Id);

            if (customerIsExist == null)
            {
                _logger.LogError("Not Found Customer with Id:{0}", request.Id);
                throw new NotFoundException("Not Found Customer With  Id:{0}", request.Id);
            }
            var customerToUpdate = _mapper.Map<Customer>(request);
            await _customerRepository.UpdateAsync(customerToUpdate);
            _logger.LogInformation("Update Customer Successfully {0}", request);

            string key = Prefix + request.Id;
            string json = JsonConvert.SerializeObject(customerToUpdate);
            await _cache.SetStringAsync(key, json);

            _logger.LogInformation("Update key {0} in Cache", key);
            
            return _mapper.Map<CustomerDto>(customerToUpdate);
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }

    }
}
