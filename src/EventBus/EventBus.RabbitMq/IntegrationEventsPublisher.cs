#region

using System;
using System.Linq;
using System.Threading.Tasks;
using EventBus.Abstractions;
using EventBus.RabbitMq.Abstractions;
using EventBus.RabbitMq.Database;
using EventBus.RabbitMq.Helpers;
using Microsoft.Extensions.Logging;

#endregion

namespace EventBus.RabbitMq
{
    public sealed class IntegrationEventsPublisher : IIntegrationEventsPublisher
    {
        private readonly IEventBus _eventBus;
        private readonly IntegrationEventDeserializer _eventDeserializer;
        private readonly ILogger<IntegrationEventsPublisher> _logger;
        private readonly IIntegrationEventRepository _repository;

        public IntegrationEventsPublisher(
            IIntegrationEventRepository repository,
            IEventBus eventBus,
            ILogger<IntegrationEventsPublisher> logger,
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
            // TODO: ordering integration event by creation date is better to be implemented on DB side

            var publishTasks =
                from entry in await _repository.GetUnpublishedEventsAsync(correlationId)
                let integrationEvent = _eventDeserializer.DeserializeFrom(entry)
                orderby integrationEvent.CreationDate
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