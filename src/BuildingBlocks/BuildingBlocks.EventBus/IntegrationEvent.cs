using System;
using System.Text.Json.Serialization;
using BuildingBlocks.Domain.DomainRules;

namespace BuildingBlocks.EventBus
{
    // Take a look here => https://railseventstore.org/docs/v2/correlation_causation/
    // to understand what are these ids  about
    
    public class IntegrationEvent
    {        
        public Guid EventId { get; }
        public Guid CorrelationId { get; }
        public Guid CausationId { get; }
        
        public DateTime CreationDate { get; }
        
        public IntegrationEvent(Guid eventId, Guid correlationId, Guid causationId)
        {
            EventId = eventId;
            CorrelationId = correlationId;
            CausationId = causationId;
            
            CreationDate = SystemClock.DateTimeNow;
        }
        
        [JsonConstructor]
        public IntegrationEvent(Guid eventId,Guid correlationId, Guid causationId, DateTime creationDate)
        {
            EventId = eventId;
            CorrelationId = correlationId;
            CausationId = causationId;
            
            CreationDate = creationDate;
        }
    }
}