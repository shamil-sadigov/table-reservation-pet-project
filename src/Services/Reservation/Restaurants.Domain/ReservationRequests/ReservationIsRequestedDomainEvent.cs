#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Visitors.ValueObjects;

#endregion

namespace Restaurants.Domain.ReservationRequests
{
    public sealed record ReservationIsRequestedDomainEvent(
            RestaurantId RestaurantId,
            TableId RequestedTableId,
            DateTime VisitingDateTime,
            VisitorId VisitorId)
        : DomainEventBase;
}