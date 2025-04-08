using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class UpdateCategoryCommand:IRequest<CategoryDto>
{
    public Guid Id { get; set; }    
    public string Name { get; set; }

    public string Description { get; set; }
}
