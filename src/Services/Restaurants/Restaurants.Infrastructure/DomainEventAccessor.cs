#region

using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainEvents;
using MoreLinq;
using Restaurants.Application.Contracts;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure
{
    public class DomainEventsAccessor : IDomainEventAccessor
    {
        private readonly RestaurantContext _context;

        public DomainEventsAccessor(RestaurantContext context)
        {
            _context = context;
        }

        public IReadOnlyCollection<IDomainEvent> GetAllDomainEvents() =>
            _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.HasDomainEvents)
                .SelectMany(x => x.Entity.DomainEvents!)
                .ToList();

        public void ClearAllDomainEvents() =>
            _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.HasDomainEvents)
                .ForEach(entity => entity.Entity.ClearDomainEvents());
    }
}