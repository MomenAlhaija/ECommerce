namespace ECommerce.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; }

    public ICollection<Product> Products { get; set; }  
}
