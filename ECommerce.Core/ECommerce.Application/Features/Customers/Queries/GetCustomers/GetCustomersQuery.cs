using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class GetCustomersQuery:IRequest<List<CustomerDto>>
{
}
