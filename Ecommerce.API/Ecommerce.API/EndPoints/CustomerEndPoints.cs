using ECommerce.Application.Features;
using MediatR;

namespace BuildingBlocks.EndPoints;

public static class CustomerEndPoints
{
    public static string apiPath = "/customers/";
    public static void CustomerEndpoints(this WebApplication app)
    {
        app.MapGroup(apiPath)
           .MapCustomersApi()
           .WithTags("Customers Api");
    }

    private static RouteGroupBuilder MapCustomersApi(this RouteGroupBuilder group)
    {
        _ = group.MapGet("allCustomers", async (IMediator mediator) =>
        {
            var result = await mediator.Send(new GetCustomersQuery());
            return result;
        });
        _ = group.MapGet("customerDetails", async (IMediator mediator, Guid id) =>
        {
            var resiult = await mediator.Send(new GetCustomerDetailsQuery { Id=id});
            return resiult;
        });
        _ = group.MapPost("addCustomer", async (IMediator mediator, CreateCustomerCommand Customer) =>
        {
            var result = await mediator.Send(Customer);
            return result;
        });
        _ = group.MapPut("updateCustomer", async (IMediator mediator, UpdateCustomerCommand Customer, Guid id) =>
        {
            Customer.Id = id;
            var result = await mediator.Send(Customer);
            return result;
        });
        _ = group.MapDelete("deleteCustomer", async (IMediator mediator, Guid id) =>
        {
            var result= await mediator.Send(new DeleteCustomerCommand { Id = id });
            return result ? Results.Ok(new { message = "Customer deleted successfully." }) :
                Results.NotFound(new { message = "Customer not found." });
        });
        return group;
    }
}
