#region

using System;
using System.Threading;
using System.Threading.Tasks;
using EventBus.Abstractions;
using MediatR;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Restaurants.DomainEvents;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestTableReservation.IntegrationEvent
{
    public class TableReservationIsRequestedIntegrationEventRegistration
        : INotificationHandler<TableReservationIsRequestedDomainEvent>
    {
        private readonly IExecutionContext _executionContext;
        private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

        public TableReservationIsRequestedIntegrationEventRegistration(
            IIntegrationEventsPublisher integrationEventsPublisher,
            IExecutionContext executionContext
        )
        {
            _integrationEventsPublisher = integrationEventsPublisher;
            _executionContext = executionContext;
        }

        public Task Handle(TableReservationIsRequestedDomainEvent notification, CancellationToken cancellationToken)
        {
            EnsureCommandIdAvailable();

            var integrationEvent = new TableReservationIsRequestedIntegrationEvent(
                _executionContext.CorrelationId,
                _executionContext.CurrentExecutingCommandId!.Value,
                notification.RestaurantId.Value,
                notification.TableId.Value,
                notification.VisitorId.Value,
                notification.VisitingDateTime);

            _integrationEventsPublisher.RegisterEventAsync(integrationEvent);

            return Task.CompletedTask;
        }

        private void EnsureCommandIdAvailable()
        {
            if (_executionContext.CurrentExecutingCommandId is null)
                throw new InvalidOperationException(
                    $"Unable table send integration event {typeof(TableReservationIsRequestedIntegrationEvent)}" +
                    "CurrentCommandId is not available");
        }
    }
}