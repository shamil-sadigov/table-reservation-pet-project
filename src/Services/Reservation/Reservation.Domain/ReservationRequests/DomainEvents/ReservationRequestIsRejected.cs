using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationRequestIsRejected(
            ReservationRequestId ReservationRequestId, 
            AdministratorId RejectedByAdministratorId, 
            DateTime RejectedDateTime,
            string RejectionReason)
        : DomainEventBase;
}