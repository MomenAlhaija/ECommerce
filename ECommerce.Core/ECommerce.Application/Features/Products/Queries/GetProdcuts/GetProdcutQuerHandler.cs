using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Models;
using ECommerce.Application.Models.Prodcuts;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace ECommerce.Application.Features;
public class GetProdcutsQuerHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProdcutDetailsQueryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Products";
    public GetProdcutsQuerHandler(IProductRepository productRepository, IMapper mapper, ILogger<GetProdcutDetailsQueryHandler> logger, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<Product> products= await _productRepository.ListAllAsync();
            _logger.LogInformation("Get Products :{0}", [products]);
            return _mapper.Map<List<ProductDto>>(products);
        }
        catch (Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}

