using MediatR;

namespace ECommerce.Application.Features;

public class DeleteCategoryCommand:IRequest<bool>
{
    public Guid Id { get; set; }    
}
