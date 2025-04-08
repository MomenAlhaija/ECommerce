using ECommerce.Application.Contract;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.Extensions.Caching.Distributed;

namespace ECommerce.Infrastructure.Repositories;

internal class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context,IDistributedCache cache) : base(context, cache)
    {
    }
}
