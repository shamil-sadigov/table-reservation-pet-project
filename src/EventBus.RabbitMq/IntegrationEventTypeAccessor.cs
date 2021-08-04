using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BuildingBlocks.EventBus;

namespace EventBus.RabbitMq
{
    // TODO: Register as singleton
    public sealed class IntegrationEventTypesAccessor
    {
        private readonly Lazy<List<Type>> _integrationEventTypes;

        public IntegrationEventTypesAccessor()
        {
            _integrationEventTypes = new Lazy<List<Type>>(() =>
            {
                return Assembly
                    .Load(Assembly.GetEntryAssembly()?.FullName)
                    .GetTypes()
                    .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
                    .ToList();
            });
        }

        public Type? FindByFullName(string name)
        {
            return _integrationEventTypes.Value.Find(x => x.FullName == name);
        }
    }
}