using BuildingBlocks.Domain.DomainEvents;
using Reservations.Domain.ReservationRequests.ValueObjects;

namespace Reservations.Domain.Visitors.DomainEvents
{
    public sealed record VisitorCreatedDomainEvent(
        VisitorId VisitorId) : DomainEventBase;
}