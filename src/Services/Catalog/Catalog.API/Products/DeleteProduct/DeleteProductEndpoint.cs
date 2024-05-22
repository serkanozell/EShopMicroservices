namespace Catalog.API.Products.DeleteProduct
{
    //public record DeleteProductRequest(Guid Id);
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("products/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteProductCommand(id));

                var response = result.Adapt<DeleteProductResponse>();

                return Results.Ok(response);
            })
             .WithName("DeleteProduct")
             .Produces<DeleteProductResponse>(statusCode: StatusCodes.Status200OK)
             .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
             .ProducesProblem(statusCode: StatusCodes.Status404NotFound)
             .WithSummary("Delete Products")
             .WithDescription("Delete Products");
        }
    }
}