using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class CreateCategoryCommand:IRequest<CategoryDto>
{
    public string Name { get; set; }

    public string Description { get; set; }
}
