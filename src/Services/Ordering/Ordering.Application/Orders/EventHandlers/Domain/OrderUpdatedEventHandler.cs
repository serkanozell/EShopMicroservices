using MassTransit;

namespace Ordering.Application.Orders.EventHandlers.Domain
{
    public class OrderUpdatedEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderUpdatedEvent> logger) : INotificationHandler<OrderUpdatedEvent>
    {
        public async Task Handle(OrderUpdatedEvent domainEvent, CancellationToken cancellationToken)
        {
            logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

            var orderUpdatedIntegrationEvent = domainEvent.order.ToOrderDto();

            await publishEndpoint.Publish(orderUpdatedIntegrationEvent, cancellationToken);
        }
    }
}