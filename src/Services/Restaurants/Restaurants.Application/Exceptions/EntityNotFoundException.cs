#region

using System;

#endregion

namespace Restaurants.Application.Exceptions
{
    public class EntityNotFoundException<TEntityId> : ApplicationException
    {
        public EntityNotFoundException(TEntityId entityId, string entityName)
        {
            EntityId = entityId ?? throw new ArgumentNullException(nameof(entityId));
            EntityName = entityName ?? throw new ArgumentNullException(nameof(entityId));
        }

        public TEntityId EntityId { get; }
        public string EntityName { get; }
    }
}