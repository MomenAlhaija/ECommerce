using ECommerce.Application.Models.Prodcuts;
using MediatR;

namespace ECommerce.Application.Features;

public class GetProductsQuery:IRequest<List<ProductDto>>
{
}
