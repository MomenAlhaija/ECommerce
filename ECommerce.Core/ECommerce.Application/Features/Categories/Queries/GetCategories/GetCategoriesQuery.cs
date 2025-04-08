using ECommerce.Application.Models;
using MediatR;

namespace ECommerce.Application.Features;
public class GetCategoriesQuery:IRequest<List<CategoryDto>>
{

}

