using System;
using BuildingBlocks.Domain.DomainEvents;

namespace Restaurants.Domain.ReservationRequests
{
    public record ReservationIsMadeEvent(
        ReservationId ReservationId,
        ReservationRequestId ReservationRequestId,
        DateTime CreatedDateTime) : DomainEventBase;
}