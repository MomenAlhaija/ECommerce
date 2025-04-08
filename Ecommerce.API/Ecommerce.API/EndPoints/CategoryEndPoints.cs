using ECommerce.Application.Features;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.EndPoints;

public static class CategoryEndPoints
{
    public static string apiPath = "/categories/";
    public static void CategoryEndpoints(this WebApplication app)
    {
        app.MapGroup(apiPath)
           .MapCategoriesApi()
           .WithTags("Categories Api");
    }

    public static RouteGroupBuilder MapCategoriesApi(this RouteGroupBuilder group)
    {
        _ = group.MapGet("allCategorys", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCategoriesQuery());
            return result;
        }).RequireAuthorization();
        _ = group.MapGet("categoryDetails", async (IMediator mediator, Guid id) =>
        {
            var resiult = await mediator.Send(new GetCategoryDetailsQuery { Id=id});
            return resiult;
        });
        _ = group.MapPost("addCategory", async ([FromServices] IMediator mediator, CreateCategoryCommand Category) =>
        {
            var result = await mediator.Send(Category);
            return result;
        });
        _ = group.MapPut("updateCategory", async (IMediator mediator, UpdateCategoryCommand Category, Guid id) =>
        {
            Category.Id = id;
            var result = await mediator.Send(Category);
            return result;
        });
        _ = group.MapDelete("deleteCategory", async (IMediator mediator, Guid id) =>
        {
            var result= await mediator.Send(new DeleteCategoryCommand { Id = id });
            return result ? Results.Ok(new { message = "Category deleted successfully." }):
                            Results.NotFound(new { message = "Category not found." });
            
        });
        return group;
    }
}
