using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class DeleteProductCommandHandler :IRequestHandler<DeleteProductCommand,bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductCommandHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string Prefix = "Product";
    public DeleteProductCommandHandler(IProductRepository productRepository, IMapper mapper, ILogger<DeleteProductCommandHandler> logger, IDistributedCache cache)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product is null)
            {
                _logger.LogError("Not Found Product with Id :{0}", request.Id);
                throw new NotFoundException("Not Found Product with Id :{0}", request.Id);
            }
            await _productRepository.DeleteAsync(product);
            _logger.LogInformation("Delete Product sucessfully Id:{0}", request.Id);

            string key = Prefix + request.Id;
            await _cache.RemoveAsync(key);
            _logger.LogInformation("Removed Key {0} into cache", key);

            return true;
        }
        catch(Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
