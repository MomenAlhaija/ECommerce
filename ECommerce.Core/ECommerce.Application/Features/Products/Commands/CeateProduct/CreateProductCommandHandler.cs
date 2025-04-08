using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models.Prodcuts;
using Microsoft.Extensions.Caching.Distributed;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Numerics;
using System.Security.AccessControl;
using static StackExchange.Redis.Role;

namespace ECommerce.Application.Features;

public class CreateProductCommandHandler : IRequestHandler<CreateProcductCommnd, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCommandHandler> _logger;
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Product";
    public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<CreateProductCommandHandler> logger, ICategoryRepository categoryRepository, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _categoryRepository = categoryRepository;
        _cache = cache;
    }

    public async Task<ProductDto> Handle(CreateProcductCommnd request, CancellationToken cancellationToken)
    {
        try
        {
            if(await _categoryRepository.GetByIdAsync(request.CategoryId) is null)
            {
                _logger.LogError("Not Found Category with ID:{0}", request.CategoryId);
                throw new NotFoundException("Not Found Category with ID:{0}", request.CategoryId);
            }
            var product = _mapper.Map<Product>(request);

            var productFromDb = await _productRepository.AddAsync(product);
            _logger.LogInformation("Add Product Suessfully {0}", product);

            string key = Prefix + productFromDb.Id;
            string json = JsonConvert.SerializeObject(productFromDb);
            await _cache.SetStringAsync(key, json);
            _logger.LogInformation("Add Key {0} into cache", key);

            return _mapper.Map<ProductDto>(product);
        }
        catch (Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
