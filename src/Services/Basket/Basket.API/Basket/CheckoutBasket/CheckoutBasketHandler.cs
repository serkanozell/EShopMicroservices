using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Basket.API.Basket.CheckoutBasket
{
    public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
    public record CheckoutBasketResult(bool IsSuccess);
    public class CheckoutBasketValidator : AbstractValidator<CheckoutBasketCommand>
    {
        public CheckoutBasketValidator()
        {
            RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
            RuleFor(x => x.BasketCheckoutDto.UserName).NotNull().WithMessage("UserName is required");
        }
    }

    public class CheckoutBasketHandler(IBasketRepository repository, IPublishEndpoint publishEndpoint) : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
    {
        public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
        {
            var basket = await repository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
            if (basket == null)
                return new CheckoutBasketResult(false);

            var eventMessage = new BasketCheckoutEvent()
            {
                UserName = command.BasketCheckoutDto.UserName,
                CustomerId = command.BasketCheckoutDto.CustomerId,
                TotalPrice = basket.TotalPrice,
                FirstName = command.BasketCheckoutDto.FirstName,
                LastName = command.BasketCheckoutDto.LastName,
                EmailAddress = command.BasketCheckoutDto.EmailAddress,
                AddressLine = command.BasketCheckoutDto.AddressLine,
                Country = command.BasketCheckoutDto.Country,
                State = command.BasketCheckoutDto.State,
                ZipCode = command.BasketCheckoutDto.ZipCode,
                CardName = command.BasketCheckoutDto.CardName,
                CardNumber = command.BasketCheckoutDto.CardNumber,
                Expiration = command.BasketCheckoutDto.Expiration,
                CVV = command.BasketCheckoutDto.CVV,
                PaymentMethod = command.BasketCheckoutDto.PaymentMethod
            };

            await publishEndpoint.Publish(eventMessage, cancellationToken);

            await repository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

            return new CheckoutBasketResult(true);
        }
    }
}