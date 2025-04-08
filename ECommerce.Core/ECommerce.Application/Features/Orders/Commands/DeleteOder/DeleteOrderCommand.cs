using MediatR;

namespace ECommerce.Application.Features;

public class DeleteOrderCommand:IRequest<bool>
{
    public Guid Id { get; set; }
}
