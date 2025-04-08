using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;
public class CreateCustomerCommand:IRequest<CustomerDto>
{
    public string Name { get; set; }
    public string Email { get; set; }
}

