#region

using System;
using System.Text.Json.Serialization;
using BuildingBlocks.Domain.DomainRules;

#endregion

namespace EventBus.Abstractions
{
    // Take a look here => https://railseventstore.org/docs/v2/correlation_causation/
    // to understand what are these ids  about

    public class IntegrationEvent
    {
        public IntegrationEvent(Guid correlationId, Guid causationId)
        {
            EventId = Guid.NewGuid();
            CorrelationId = correlationId;
            CausationId = causationId;

            CreationDate = SystemClock.DateTimeNow;
        }

        [JsonConstructor]
        internal IntegrationEvent(Guid eventId, Guid correlationId, Guid causationId, DateTime creationDate)
        {
            EventId = eventId;
            CorrelationId = correlationId;
            CausationId = causationId;

            CreationDate = creationDate;
        }

        public Guid EventId { get; }
        public Guid CorrelationId { get; }
        public Guid CausationId { get; }

        public DateTime CreationDate { get; }
    }
}