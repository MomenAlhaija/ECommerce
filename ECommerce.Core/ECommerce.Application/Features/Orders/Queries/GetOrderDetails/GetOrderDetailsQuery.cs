using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class GetOrderDetailsQuery:IRequest<OrderDto>
{
    public Guid Id { get; set; }
}
