using ECommerce.Application.Models.Prodcuts;
using MediatR;

namespace ECommerce.Application.Features;

public class UpdateProductCommand:IRequest<ProductDto>
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }

}
