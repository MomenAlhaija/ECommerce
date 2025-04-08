using ECommerce.Application.Models.Prodcuts;
using MediatR;

namespace ECommerce.Application.Features;

public class GetProdcutDetailsQuery:IRequest<ProductDto>
{
    public Guid Id { get; set; }
}

