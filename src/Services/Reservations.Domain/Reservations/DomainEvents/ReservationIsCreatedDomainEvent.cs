#region

using System;
using BuildingBlocks.Domain.DomainEvents;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Domain.Reservations.DomainEvents
{
    public sealed record ReservationIsCreatedDomainEvent
    (
        ReservationId ReservationId,
        ReservationRequestId ReservationRequestId,
        AdministratorId ApprovedByAdministratorId,
        RestaurantId RestaurantId,
        TableId TableId,
        VisitorId VisitorId,
        DateTime ApprovedDateTime
    ) : DomainEventBase;
}