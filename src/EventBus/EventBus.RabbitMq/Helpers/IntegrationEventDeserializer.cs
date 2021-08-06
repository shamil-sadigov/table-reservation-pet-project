#region

using System.Text.Json;
using EventBus.Abstractions;

#endregion

namespace EventBus.RabbitMq.Helpers
{
    public sealed class IntegrationEventDeserializer
    {
        private readonly IntegrationEventTypesAccessor _eventTypes;

        public IntegrationEventDeserializer(IntegrationEventTypesAccessor eventTypes)
        {
            _eventTypes = eventTypes;
        }

        public IntegrationEvent? DeserializeFrom(IntegrationEventEntry entry)
        {
            var typeToDeserialize = _eventTypes.FindByFullName(entry.EventType);

            if (typeToDeserialize is null)
                return null;

            return JsonSerializer.Deserialize(entry.EventContent, typeToDeserialize) as IntegrationEvent;
        }
    }
}