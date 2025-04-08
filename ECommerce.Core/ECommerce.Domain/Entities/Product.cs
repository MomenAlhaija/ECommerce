namespace ECommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }    
    public Guid CategoryId { get; set; }    
    public string Name { get;  set; } = default!;
    public decimal Price { get;  set; } 
}
