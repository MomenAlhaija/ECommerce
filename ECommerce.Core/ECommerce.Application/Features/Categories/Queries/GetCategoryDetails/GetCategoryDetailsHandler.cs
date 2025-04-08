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

public class GetCategoryDetailsHandler : IRequestHandler<GetCategoryDetailsQuery, CategoryDto>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCategoryDetailsHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string prefix = "Category";

    public GetCategoryDetailsHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetCategoryDetailsHandler> logger, IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<CategoryDto> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            string key = prefix + request.Id;
            Category category;
            if (await _cache.GetStringAsync(key)!=null)
            {
                string value = await _cache.GetStringAsync(key);
                category = JsonConvert.DeserializeObject<Category>(value);
                _logger.LogInformation("Get {0} from Cache", key);
            }
            else
            {
                category = await _categoryRepository.GetByIdAsync(request.Id);
                if (category is null)
                {
                    _logger.LogError("Not Found Category with Id:{0}", request.Id);
                    throw new NotFoundException("Not Found Category");
                }
                _logger.LogInformation("Found Category{0} By Id {1} ", request.Id, category);

                string json = JsonConvert.SerializeObject(category);
                await _cache.SetStringAsync(key, json);
            }
            return _mapper.Map<CategoryDto>(category);
        }
        catch(Exception ex)
        {
            _logger.LogCritical("Error Happines {0}", ex.Message);
            throw;
        }
    }
}
