#region

using BuildingBlocks.Domain.DomainEvents;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Domain.Visitors.DomainEvents
{
    public sealed record VisitorCreatedDomainEvent(
        VisitorId VisitorId) : DomainEventBase;
}