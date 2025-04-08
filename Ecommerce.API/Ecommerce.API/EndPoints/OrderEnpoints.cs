using ECommerce.Application.Features;
using MediatR;

namespace BuildingBlocks.EndPoints;

public static class OrderEnpoints
{
    public static string apiPath = "/orders/";
    public static void OrderEndpoints(this WebApplication app)
    {
        app.MapGroup(apiPath)
           .MapOrdersApi()
           .WithTags("Orders Api");
    }

    public static RouteGroupBuilder MapOrdersApi(this RouteGroupBuilder group)
    {
        _ = group.MapGet("allOrders", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetOrdersQuey());
            return result;
        });
        _ = group.MapGet("orderDetails", async (IMediator mediator, Guid id) =>
        {
            var resiult = await mediator.Send(new GetOrderDetailsQuery { Id=id});
            return resiult;
        });
        _ = group.MapPost("addOrder", async (IMediator mediator, CreateOrderCommand Order) =>
        {
            var result = await mediator.Send(Order);
            return result;
        });
        _ = group.MapPut("updateOrder", async (IMediator mediator, UpdateOrderCommand Order, Guid id) =>
        {
            Order.Id = id;
            var result = await mediator.Send(Order);
            return result;
        });
        _ = group.MapDelete("deleteOrder", async (IMediator mediator, Guid id) =>
        {
           var result= await mediator.Send(new DeleteOrderCommand { Id = id });
            return result ? Results.Ok(new { message = "Order deleted successfully." }) :
                Results.NotFound(new { message = "Order not found." });
        });
        return group;
    }
}
