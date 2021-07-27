#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationIsRequestedDomainEvent(
            ReservationRequestId ReservationRequestId,
            TableId RequestedTableId,
            NumberOfSeats NumberOfRequestedSeats,
            DateTime VisitingDateTime,
            VisitorId VisitorId)
        : DomainEventBase;
}