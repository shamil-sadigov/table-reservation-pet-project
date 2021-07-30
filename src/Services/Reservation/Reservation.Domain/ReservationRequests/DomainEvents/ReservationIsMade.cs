#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationIsMade(
            ReservationRequestId ReservationRequestId,
            AdministratorId ApprovedByAdministratorId,
            DateTime ApprovalDateTime)
        : DomainEventBase;
}