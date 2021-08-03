#region

using BuildingBlocks.Domain.DomainEvents;
using Restaurants.Domain.Visitors.ValueObjects;

#endregion

namespace Restaurants.Domain.Visitors.DomainEvents
{
    public sealed record VisitorCreatedDomainEvent(
        VisitorId VisitorId) : DomainEventBase;
}