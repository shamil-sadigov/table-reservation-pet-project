using System.Collections.Generic;
using BuildingBlocks.Domain.DomainEvents;

namespace Restaurants.Application
{
    public interface IDomainEventAccessor
    {
        IReadOnlyCollection<IDomainEvent> GetAllDomainEvents();
        void ClearAllDomainEvents();
    }
}