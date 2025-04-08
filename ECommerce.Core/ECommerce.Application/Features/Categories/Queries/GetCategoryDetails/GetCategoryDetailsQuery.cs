using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;

public class GetCategoryDetailsQuery:IRequest<CategoryDto>
{
    public Guid Id { get; set; }    
}
