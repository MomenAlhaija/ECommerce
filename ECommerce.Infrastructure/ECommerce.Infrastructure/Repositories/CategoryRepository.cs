using ECommerce.Application.Contract;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ECommerce.Infrastructure.Repositories;

public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
{
    
    public CategoryRepository(AppDbContext context, IDistributedCache cache) : base(context,cache)
    {
    }
    

}
