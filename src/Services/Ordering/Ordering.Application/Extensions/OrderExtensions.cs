namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {
            return orders.Select(order => new OrderDto(
                Id: order.Id.Value,
                CustomerId: order.CustomerId.Value,
                OrderName: order.OrderName.Value,
                ShippingAddress: new AddressDto(FirstName: order.ShippingAddress.FirstName,
                                                LastName: order.ShippingAddress.LastName,
                                                EmailAddress: order.ShippingAddress.Email,
                                                AddressLine: order.ShippingAddress.AddressLine,
                                                Country: order.ShippingAddress.Country,
                                                State: order.ShippingAddress.State,
                                                ZipCode: order.ShippingAddress.ZipCode),
                BillingAddress: new AddressDto(FirstName: order.BillingAddress.FirstName,
                                               LastName: order.BillingAddress.LastName,
                                               EmailAddress: order.BillingAddress.Email,
                                               AddressLine: order.BillingAddress.AddressLine,
                                               Country: order.BillingAddress.Country,
                                               State: order.BillingAddress.State,
                                               ZipCode: order.BillingAddress.ZipCode),
                Payment: new PaymentDto(CardName: order.Payment.CardName,
                                        CardNumber: order.Payment.CardNumber,
                                        Expiration: order.Payment.Expiration,
                                        Cvv: order.Payment.CVV,
                                        PaymentMethod: order.Payment.PaymentMethod),
                OrderStatus: order.Status,
                OrderItems: order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value,
                                                                           oi.ProductId.Value,
                                                                           oi.Quantity,
                                                                           oi.Price))
                                            .ToList()
                ));
        }
    }
}