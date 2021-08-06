using System;

namespace Restaurants.Application
{
    public class EntityNotFoundException<TEntityId> : ApplicationException
    {
        public TEntityId EntityId { get; }
        public string EntityName { get; }

        public EntityNotFoundException(TEntityId entityId, string entityName)
        {
            EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityId));
        }
    }
}