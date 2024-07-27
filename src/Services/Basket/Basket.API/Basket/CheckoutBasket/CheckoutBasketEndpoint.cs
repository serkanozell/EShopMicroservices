using Marten.Events.Daemon;
using System.Net.Mail;
using System.Reflection.Emit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketRequest(BasketCheckoutDto BasketCheckoutDto);
    public record CheckoutBasketResponse(bool IsSuccess);

    public class CheckoutBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("basket/checkout", async (CheckoutBasketRequest request, ISender sender) =>
            {
                var basketCheckoutDto = new BasketCheckoutDto(UserName: request.BasketCheckoutDto.UserName,
                                                              CustomerId: request.BasketCheckoutDto.CustomerId,
                                                              TotalPrice: 0,
                                                              FirstName: request.BasketCheckoutDto.FirstName,
                                                              LastName: request.BasketCheckoutDto.LastName,
                                                              EmailAddress: request.BasketCheckoutDto.EmailAddress,
                                                              AddressLine: request.BasketCheckoutDto.AddressLine,
                                                              Country: request.BasketCheckoutDto.Country,
                                                              State: request.BasketCheckoutDto.State,
                                                              ZipCode: request.BasketCheckoutDto.ZipCode,
                                                              CardName: request.BasketCheckoutDto.CardName,
                                                              CardNumber: request.BasketCheckoutDto.CardNumber,
                                                              Expiration: request.BasketCheckoutDto.Expiration,
                                                              CVV: request.BasketCheckoutDto.CVV,
                                                              PaymentMethod: request.BasketCheckoutDto.PaymentMethod);

                var command = new CheckoutBasketCommand(BasketCheckoutDto: basketCheckoutDto);

                var result = await sender.Send(command);

                var response = result.Adapt<CheckoutBasketResponse>();

                return Results.Ok(response);
            })
             .WithName("CheckoutBasket")
             .Produces<CheckoutBasketResponse>(statusCode: StatusCodes.Status201Created)
             .ProducesProblem(statusCode: StatusCodes.Status400BadRequest)
             .WithSummary("Checkout Basket")
             .WithDescription("Checkout Basket");
        }
    }
}