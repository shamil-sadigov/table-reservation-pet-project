#region

using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.Visitors.DomainEvents
{
    public sealed record VisitorCreatedDomainEvent(
        VisitorId VisitorId) : DomainEventBase;
}