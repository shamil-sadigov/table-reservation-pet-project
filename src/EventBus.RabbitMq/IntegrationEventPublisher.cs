#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.EventBus;
using Microsoft.Extensions.Logging;

#endregion

namespace EventBus.RabbitMq
{
    public sealed class IntegrationEventPublisher : IIntegrationEventPublisher
    {
        private readonly IEventBus _eventBus;
        private readonly List<Type> _eventTypes;
        private readonly ILogger<IntegrationEventPublisher> _logger;
        private readonly IntegrationEventDeserializer _eventDeserializer;
        private readonly IIntegrationEventRepository _repository;

        public IntegrationEventPublisher(
            IIntegrationEventRepository repository,
            IEventBus eventBus,
            ILogger<IntegrationEventPublisher> logger,
            IntegrationEventDeserializer eventDeserializer)
        {
            _repository = repository;
            _eventBus = eventBus;
            _logger = logger;
            _eventDeserializer = eventDeserializer;
        }

        
        public async Task RegisterEventAsync(IntegrationEvent @event)
        {
            var entry = new IntegrationEventEntry(@event);
            await _repository.AddAsync(entry);
        }

        public async Task PublishEventsAsync(Guid correlationId)
        {
            // TODO: Add logging here
            var publishTasks = 
                from entry in await _repository.GetUnpublishedEventsAsync(correlationId)
                let integrationEvent = _eventDeserializer.DeserializeFrom(entry)
                select Task.Run(() =>
                {
                    try
                    {
                        // TODO: Add logging here
                        _eventBus.Publish(integrationEvent);
                        MarkAsPublished(entry); 
                    }
                    catch (Exception e)
                    {
                        // TODO: Add logging here
                        MarkAsFailed(entry);
                    }
                });
            
            await Task.WhenAll(publishTasks);
        }

        private void MarkAsPublished(IntegrationEventEntry entry)
        {
            entry.Published();
            _repository.Update(entry);
        }
        
        private void MarkAsFailed(IntegrationEventEntry entry)
        {
            entry.Failed();
            _repository.Update(entry);
        }
    }
}