using System;

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.IntegrationEvent
{
    public class TableReservationIsRequestedIntegrationEvent:BuildingBlocks.EventBus.IntegrationEvent
    {
        public Guid RestaurantId { get; }
        public string TableId { get; }
        public DateTime VisitingDateTime { get; }
        public Guid VisitorId { get; }

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
    }
}