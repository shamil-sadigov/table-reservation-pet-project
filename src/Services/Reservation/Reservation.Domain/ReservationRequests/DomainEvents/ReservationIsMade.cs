using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.ReservationRequests.DomainEvents
{
    public sealed record ReservationIsMade(
            ReservationRequestId ReservationRequestId, 
            AdministratorId ApprovedByAdministratorId, 
            DateTime ApprovalDateTime)
        : DomainEventBase;
}