#region

using System;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.IntegrationEvent
{
    public class TableReservationIsRequestedIntegrationEvent : EventBus.Abstractions.IntegrationEvent
    {
        public TableReservationIsRequestedIntegrationEvent(
            Guid correlationId,
            Guid causationId,
            Guid restaurantId,
            string tableId,
            Guid visitorId,
            DateTime visitingDateTime) : base(correlationId, causationId)
        {
            RestaurantId = restaurantId;
            TableId = tableId;
            VisitingDateTime = visitingDateTime;
            VisitorId = visitorId;
        }

        public Guid RestaurantId { get; }
        public string TableId { get; }
        public DateTime VisitingDateTime { get; }
        public Guid VisitorId { get; }
    }
}