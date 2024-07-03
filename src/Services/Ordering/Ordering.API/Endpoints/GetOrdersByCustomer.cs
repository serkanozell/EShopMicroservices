using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints
{
    public record GetOrdersByCustomerRequest(Guid CustomerId);
    public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

    public class GetOrdersByCustomer : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders/customer/{customerId}", async (Guid customerId, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersByCustomerQuery(customerId));

                var response = result.Adapt<GetOrdersByCustomerResponse>();

                return Results.Ok(response);
            })
             .WithName("GetOrdersByCustomer")
             .Produces<GetOrdersByCustomerResponse>(statusCode: StatusCodes.Status200OK)
             .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
             .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
             .WithSummary("Get Orders By Customer")
             .WithDescription("Get Orders By Customer");
        }
    }
}