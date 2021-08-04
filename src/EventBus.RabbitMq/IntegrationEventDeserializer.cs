using System.Text.Json;
using BuildingBlocks.EventBus;

namespace EventBus.RabbitMq
{
    // TODO: register as singleton
    public sealed class IntegrationEventDeserializer
    {
        private readonly IntegrationEventTypesAccessor _typesAccessor;

        public IntegrationEventDeserializer(IntegrationEventTypesAccessor typesAccessor)
        {
            _typesAccessor = typesAccessor;
        }

        public IntegrationEvent? DeserializeFrom(IntegrationEventEntry entry)
        {
            var typeToDeserialize = _typesAccessor.FindByFullName(entry.EventType);

            if (typeToDeserialize is null)
                return null;
            
            return JsonSerializer.Deserialize(entry.EventContent, typeToDeserialize) as IntegrationEvent;
        }
    }
}