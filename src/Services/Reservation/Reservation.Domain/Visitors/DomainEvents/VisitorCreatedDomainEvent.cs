using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Visitors.ValueObjects;

namespace Reservation.Domain.Visitors.DomainEvents
{
    public sealed record VisitorCreatedDomainEvent(
        VisitorId VisitorId) : DomainEventBase;
}