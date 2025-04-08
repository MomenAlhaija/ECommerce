using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models.Prodcuts;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class GetProdcutDetailsQueryHandler : IRequestHandler<GetProdcutDetailsQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProdcutDetailsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Order";
    public GetProdcutDetailsQueryHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProdcutDetailsQueryHandler> logger, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<ProductDto> Handle(GetProdcutDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Product product;
            string key = Prefix + request.Id;
            if (await _cache.GetStringAsync(key)!=null)
            {
                string value = await _cache.GetStringAsync(key);
                product = JsonConvert.DeserializeObject<Product>(value);
                return _mapper.Map<ProductDto>(product);
            }
            else 
            {
                product = await _productRepository.GetByIdAsync(request.Id);
                if (product is null)
                {
                    _logger.LogError($"Not Found Product With Id :{request.Id}");
                    throw new NotFoundException($"Not Found Product With Id :{request.Id}");
                }
                _logger.LogInformation("Get Order:{0} by Id:{1} ", request, request.Id);

                string json = JsonConvert.SerializeObject(product);
                await _cache.SetStringAsync(key, json);
                _logger.LogInformation("Add Key {0} into cahe", key);
                
                return _mapper.Map<ProductDto>(product);
            }
        }
        catch (Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
