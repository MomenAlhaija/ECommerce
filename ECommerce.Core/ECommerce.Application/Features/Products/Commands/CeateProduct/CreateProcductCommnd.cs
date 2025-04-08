using ECommerce.Application.Models.Prodcuts;
using MediatR;

namespace ECommerce.Application.Features;
public class CreateProcductCommnd:IRequest<ProductDto>
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }

}

