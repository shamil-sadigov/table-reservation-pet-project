#region

using System;
using System.Text.Json;
using BuildingBlocks.Domain.DomainRules;
using EventBus.Abstractions;

#endregion

namespace EventBus.RabbitMq.Database
{
    public class IntegrationEventEntry
    {
        // for EF
        private IntegrationEventEntry()
        {
            
        }
        
        public IntegrationEventEntry(IntegrationEvent @event)
        {
            EventId = @event.EventId;
            CorrelationId = @event.CorrelationId;
            CausationId = @event.CausationId;

            CreationDate = @event.CreationDate;
            EventType = @event.GetType().FullName;
            EventContent = JsonSerializer.Serialize(@event, @event.GetType());
            State = IntegrationEventState.Unpublished;
        }

        public Guid EventId { get; }
        public Guid CausationId { get; }
        public Guid CorrelationId { get; }

        public DateTime CreationDate { get; }

        public DateTime? PublishedDateTime { get; private set; }

        public string EventType { get; }
        public string EventContent { get; }
        public IntegrationEventState State { get; private set; }

        public void Published()
        {
            State = IntegrationEventState.Published;
            PublishedDateTime = SystemClock.DateTimeNow;
        }

        public void Failed()
        {
            State = IntegrationEventState.Failed;
        }
    }
}