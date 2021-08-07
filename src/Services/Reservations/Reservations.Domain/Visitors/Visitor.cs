#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Visitors.DomainEvents;

#endregion

namespace Reservations.Domain.Visitors
{
    /// <summary>
    ///     Person who creates reservation request to visit a restaurant.
    /// </summary>
    public class Visitor : Entity, IAggregateRoot
    {
        // Additional data will be added

        // For EF
        private Visitor()
        {
        }

        private Visitor(VisitorId visitorId)
        {
            Id = visitorId;
            AddDomainEvent(new VisitorCreatedDomainEvent(Id));
        }

        public VisitorId Id { get; }

        public static Result<Visitor> TryCreate(VisitorId visitorId)
        {
            if (ContainsNullValues(new {visitorId}, out var errors))
                return errors;

            return new Visitor(visitorId);
        }
    }
}