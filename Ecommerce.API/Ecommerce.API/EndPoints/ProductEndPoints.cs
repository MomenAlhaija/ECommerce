using ECommerce.Application.Features;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using StackExchange.Redis;

namespace BuildingBlocks.EndPoints;

public static class ProductEndPoints
{
    public const string apiPath = "/products/";
    public static void ProductEndpoints(this WebApplication app)
    {
        app.MapGroup(apiPath)
           .MapProductsApi()
           .WithTags("Products Api");
    }

    private static RouteGroupBuilder MapProductsApi(this RouteGroupBuilder group)
    {
        _ = group.MapGet("allProducts",async(IMediator mediator)=>
        {
            var result = await mediator.Send(new GetProductsQuery());
            return result;
        });
        _ = group.MapGet("productDetails", async (IMediator mediator,[FromQuery] Guid id) =>
        {
            var resiult = await mediator.Send(new GetProdcutDetailsQuery { Id=id});
            return resiult;
        });
        _ = group.MapPost("addProduct", async (IMediator mediator, CreateProcductCommnd product) =>
        {
            var result= await mediator.Send(product);
            return result;
        });
        _ = group.MapPut("updateProduct", async (IMediator mediator,UpdateProductCommand product, Guid id) =>
        {
            product.Id = id;
            var result= await mediator.Send(product);
            return result;
        });
        _ = group.MapDelete("deleteProduct", async (IMediator mediator, Guid id) =>
        {
            var result=  await mediator.Send(new DeleteProductCommand { Id = id });
            return result ? Results.Ok(new { message = "Product deleted successfully." }) :
                            Results.NotFound(new { message = "Product not found." });
        });
        return group;
    }
}