using ECommerce.Application.Contract;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace ECommerce.Infrastructure.Repositories;

public class BaseRepository<T> : IAsyncRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly IDistributedCache _cache;
    public BaseRepository(AppDbContext context, IDistributedCache cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<T>? GetByIdAsync(Guid id)
    {
        return await _context.Set<T>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(entity => EF.Property<Guid>(entity, "Id") == id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateListCacheAsync(string prefix)
    {
        var list = await ListAllAsync();
        string value = JsonConvert.SerializeObject(list);
        await _cache.SetStringAsync(prefix, value);
    }
}

