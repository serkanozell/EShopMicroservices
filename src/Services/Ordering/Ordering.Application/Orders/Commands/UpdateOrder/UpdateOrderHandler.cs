namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);

            var order = await dbContext.Orders.FindAsync([orderId], cancellationToken: cancellationToken);

            if (order is null)
                throw new OrderNotFoundException(command.Order.Id);

            UpdateOrderWithNewValues(order, command.Order);

            dbContext.Orders.Update(order);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var updatedShippingAddress = Address.Of(firstName: orderDto.ShippingAddress.FirstName,
                                                    lastName: orderDto.ShippingAddress.LastName,
                                                    emailAddress: orderDto.ShippingAddress.EmailAddress,
                                                    addressLine: orderDto.ShippingAddress.AddressLine,
                                                    country: orderDto.ShippingAddress.Country,
                                                    state: orderDto.ShippingAddress.State,
                                                    zipCode: orderDto.ShippingAddress.ZipCode);

            var updatedBillingAddress = Address.Of(firstName: orderDto.BillingAddress.FirstName,
                                                   lastName: orderDto.BillingAddress.LastName,
                                                   emailAddress: orderDto.BillingAddress.EmailAddress,
                                                   addressLine: orderDto.BillingAddress.AddressLine,
                                                   country: orderDto.BillingAddress.Country,
                                                   state: orderDto.BillingAddress.State,
                                                   zipCode: orderDto.BillingAddress.ZipCode);

            var updatedPayment = Payment.Of(cardName: orderDto.Payment.CardName,
                                            cardNumber: orderDto.Payment.CardNumber,
                                            expiration: orderDto.Payment.Expiration,
                                            cvv: orderDto.Payment.Cvv,
                                            paymentMethod: orderDto.Payment.PaymentMethod);

            order.Update(orderName: OrderName.Of(orderDto.OrderName),
                         shippingAddress: updatedShippingAddress,
                         billingAddress: updatedBillingAddress,
                         payment: updatedPayment,
                         orderStatus: orderDto.OrderStatus);
        }
    }
}