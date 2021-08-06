#region

using System.Collections.Generic;
using BuildingBlocks.Domain.DomainEvents;

#endregion

namespace Restaurants.Application
{
    public interface IDomainEventAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();
        void ClearAllDomainEvents();
    }
}