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

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateCategoryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string prefix = "Category";
    public UpdateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<UpdateCategoryHandler> logger, IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CategoryDto> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Category category;
            if (await _cache.GetStringAsync(request.Id.ToString()) is not null)
            {
               string value = await _cache.GetStringAsync(request.Id.ToString());
                category = JsonConvert.DeserializeObject<Category>(value);
            }
            else
            {
                category = await _categoryRepository.GetByIdAsync(request.Id);
            }

            if (category is null)
            {
                _logger.LogError("Not Found Category With Id:{0}", request.Id);
                throw new NotFoundException("Not Found Category With Id:{0}", request.Id);
            }
            var categoryToUpdate = _mapper.Map<Category>(request);


            await _categoryRepository.UpdateAsync(categoryToUpdate);
            _logger.LogInformation("Updated Category with Id {0} And Body:{1}", request.Id, request);

            string cacheKey = prefix + request.Id;
            string categoryJson = JsonConvert.SerializeObject(categoryToUpdate);
            await  _cache.SetStringAsync(cacheKey, categoryJson);


            return _mapper.Map<CategoryDto>(categoryToUpdate);
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
