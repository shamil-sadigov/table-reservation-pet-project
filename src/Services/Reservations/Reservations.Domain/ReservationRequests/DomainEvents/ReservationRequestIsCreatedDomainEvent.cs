#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationRequestIsCreatedDomainEvent(
        ReservationRequestId ReservationRequestId,
        RestaurantId VisitingDateTime,
        TableId TableId,
        DateTime CreatedDateTime,
        DateTime DateTime,
        VisitorId VisitorId) : DomainEventBase;
}