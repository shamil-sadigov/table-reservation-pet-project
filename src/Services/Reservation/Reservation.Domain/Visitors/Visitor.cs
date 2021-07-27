using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.Visitors.DomainEvents;
using Reservation.Domain.Visitors.ValueObjects;

namespace Reservation.Domain.Visitors
{
    /// <summary>
    /// Person who creates reservation request to visit a restaurant.
    /// </summary>
    public class Visitor : Entity, IAggregateRoot
    {
        private VisitorId _visitorId;
        
        // Additional data will be added
        
        // For EF
        private Visitor(VisitorId visitorId)
        {
            _visitorId = visitorId;
            
            AddDomainEvent(new VisitorCreatedDomainEvent(_visitorId));
        }
        
        public static Result<Visitor> TryCreate(VisitorId visitorId)
        {
            if (ContainsNullValues(new {visitorId}, out var errors))
                return errors;

            return new Visitor(visitorId);
        }
    }
}