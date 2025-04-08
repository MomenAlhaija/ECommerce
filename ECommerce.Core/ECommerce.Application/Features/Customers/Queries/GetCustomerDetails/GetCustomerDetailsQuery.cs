using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;
public class GetCustomerDetailsQuery:IRequest<CustomerDto>
{
    public Guid Id { get; set; }

}
