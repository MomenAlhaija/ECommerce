

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

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Product";
    public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<UpdateProductCommandHandler> logger, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                _logger.LogError($"Not Found Product With Id:{request.Id}");
                throw new NotFoundException($"Not Found Product With Id:{request.Id}");
            }

            var produtToUpdate = _mapper.Map<Product>(request);
            await _productRepository.UpdateAsync(produtToUpdate);
            _logger.LogInformation("Update product Succcessfully {0}", product);

            string key = Prefix + produtToUpdate.Id;
            string json = JsonConvert.SerializeObject(produtToUpdate);
            await _cache.SetStringAsync(key, json);
            _logger.LogInformation("Updtae Key {0} into cache", key);

            return _mapper.Map<ProductDto>(produtToUpdate);
        }
        catch (Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
