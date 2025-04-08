using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class GetOrdersQuey:IRequest<List<OrderDto>>
{
}
