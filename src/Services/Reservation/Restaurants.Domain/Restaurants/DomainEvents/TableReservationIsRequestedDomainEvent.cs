#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Visitors.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.DomainEvents
{
    public sealed record TableReservationIsRequestedDomainEvent(
            RestaurantId RestaurantId,
            TableId TableId,
            DateTime VisitingDateTime,
            VisitorId VisitorId)
        : DomainEventBase;
}