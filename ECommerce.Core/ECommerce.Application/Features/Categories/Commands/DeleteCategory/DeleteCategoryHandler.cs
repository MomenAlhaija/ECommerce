using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Numerics;

namespace ECommerce.Application.Features;

public class DeleteCategoryHandler:IRequestHandler<DeleteCategoryCommand,bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteCategoryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string prefix = "Category";

    public DeleteCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<DeleteCategoryHandler> logger, IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if(category is null)
            {
                _logger.LogError("Not Fount Category With ID:{0}", request.Id);
                throw new NotFoundException("Not Found Category");
            }
            await _categoryRepository.DeleteAsync(category);
            _logger.LogInformation("Deleted Category Sucessfuly {0}",category);

            string cacheKey = prefix +request.Id;
            await _cache.RemoveAsync(cacheKey);
            _logger.LogInformation("Remove {0} from Cache", cacheKey);
            return true;
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Throw Exception Error {0}", ex.Message);
            throw;
        }
    }
}
