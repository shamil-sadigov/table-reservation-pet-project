#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BuildingBlocks.EventBus;

#endregion

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
                return AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetExportedTypes())
                    .Where(x => typeof(IntegrationEvent).IsAssignableFrom(x))
                    .ToList();
            });
        }
        
        public IntegrationEventTypesAccessor(Assembly integrationEventAssembly)
        {
            _integrationEventTypes = new Lazy<List<Type>>(() =>
            {
                return integrationEventAssembly
                    .GetExportedTypes()
                    .Where(x => typeof(IntegrationEvent).IsAssignableFrom(x))
                    .ToList();
            });
        }
        
        public IntegrationEventTypesAccessor(Func<List<Type>> integrationEventsProvider)
        {
            _integrationEventTypes = new Lazy<List<Type>>(integrationEventsProvider);
        }

        public Type? FindByFullName(string name)
        {
            return _integrationEventTypes.Value.Find(x => x.FullName == name);
        }
    }
}