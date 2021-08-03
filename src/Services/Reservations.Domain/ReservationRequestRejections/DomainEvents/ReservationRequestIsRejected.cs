#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequestRejections.DomainEvents
{
    public sealed record ReservationRequestIsRejected(
            ReservationRequestRejectionId ReservationRequestRejectionId,
            ReservationRequestId ReservationRequestId,
            AdministratorId RejectedByAdministratorId,
            DateTime RejectedDateTime,
            RejectionReason RejectionReason)
        : DomainEventBase;
}