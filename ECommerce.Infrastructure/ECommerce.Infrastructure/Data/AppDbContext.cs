using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ECommerce.Infrastructure.Data;

public class AppDbContext:DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
    {
            
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    public DbSet<Order> Orders { get; set; }        
    public DbSet<Product> Products { get; set; }    
    public DbSet<OrderItem> OrderItems { get; set; }    
    public DbSet<Customer> Customers { get; set; }    
    public DbSet<Category> Categories { get; set; } 
}
