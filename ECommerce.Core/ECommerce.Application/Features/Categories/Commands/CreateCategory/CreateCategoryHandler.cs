using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Exceptions;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Numerics;
using System.Text.Json.Serialization;

namespace ECommerce.Application.Features;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand,CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateCategoryHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string prefix = "Category";
    public CreateCategoryHandler(ICategoryRepository categoryRepository, 
                               IMapper mapper, 
                               ILogger<CreateCategoryHandler> logger,
                               IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var category = _mapper.Map<Category>(request);
            if (category is Category)
            {
                var categoryFromDb= await _categoryRepository.AddAsync(category);
                _logger.LogInformation("Create Catgory Succesfully wih Body:{0}", category);

                string cacheKey = prefix + categoryFromDb.Id.ToString();
                var categoryJson = JsonConvert.SerializeObject(categoryFromDb);

                await _cache.SetStringAsync(cacheKey, categoryJson);
                _logger.LogInformation("Added {0} into Cache", cacheKey);

                return _mapper.Map<CategoryDto>(categoryFromDb);
            }
            _logger.LogError("Can't Create Invalid Category With Body {0}", category);
            throw new BadRequestException("Invalid Category");
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Throw Exception Error {0} ",ex.Message);
            throw;
        }
    }
    

}
