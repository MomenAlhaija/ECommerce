using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class UpdateCustomerCommand:IRequest<CustomerDto>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}
