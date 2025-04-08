using MediatR;

namespace ECommerce.Application.Features;

public class DeleteCustomerCommand:IRequest<bool>
{
    public Guid Id { get; set; }
}
