#region

using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain.DomainEvents;

#endregion

namespace BuildingBlocks.Domain
{
    public abstract class Entity
    {
        private List<IDomainEvent>? _domainEvents;

        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        public bool HasDomainEvents => _domainEvents?.Any() is true;

        public void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();

            _domainEvents.Add(domainEvent);
        }

        // 😈
        protected static bool ContainsNullValues(object obj, out List<Error> errors)
        {
            return NullChecker.TryCheckNullValues(obj, out errors);
        }
    }
}