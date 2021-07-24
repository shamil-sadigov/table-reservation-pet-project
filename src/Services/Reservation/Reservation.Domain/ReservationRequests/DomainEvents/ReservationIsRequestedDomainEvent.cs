using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

namespace Reservation.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationIsRequestedDomainEvent(
            ReservationRequestId ReservationRequestId,
            TableId RequestedTableId)
        : DomainEventBase;
}