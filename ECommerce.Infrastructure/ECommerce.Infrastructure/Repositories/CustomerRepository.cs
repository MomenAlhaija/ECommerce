using ECommerce.Application.Contract;
using ECommerce.Domain.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace ECommerce.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<CustomerRepository> _logger;
    public CustomerRepository(AppDbContext context,IDistributedCache cache, ILogger<CustomerRepository> logger) : base(context,cache)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<bool> EmailIsAlreadyExist(Customer customer)
    {
        try {
            return _context.Customers.AsNoTracking()
                    .Any(p => p.Email == customer.Email
                     && ((customer.Id == Guid.Empty) || (customer.Id != p.Id)));

   
        }
        catch(Exception ex)
        {

            _logger.LogCritical("Error Happines {0}", ex.Message);
throw;
        }
        
    }
}
