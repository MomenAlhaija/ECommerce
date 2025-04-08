using AutoMapper;
using ECommerce.Application.Contract;
using ECommerce.Application.Models;
using ECommerce.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ECommerce.Application.Features;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetCategoriesHandler> _logger;
    private readonly IDistributedCache _cache;
    private const string prefix = "Categories";
    public GetCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetCategoriesHandler> logger, IDistributedCache cache)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
        _cache = cache;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<Category> categories= await _categoryRepository.ListAllAsync();
            string json = JsonConvert.SerializeObject(categories);            
            return _mapper.Map<List<CategoryDto>>(categories);
        }
        catch(Exception ex)
        {
            _logger.LogError("Error HAppines {0}", ex.Message);
            throw;
        }
    }
}
