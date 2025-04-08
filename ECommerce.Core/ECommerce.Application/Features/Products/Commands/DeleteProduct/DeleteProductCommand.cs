using MediatR;

namespace ECommerce.Application.Features;

public class DeleteProductCommand:IRequest<bool>
{
    public Guid Id { get; set; }
}
