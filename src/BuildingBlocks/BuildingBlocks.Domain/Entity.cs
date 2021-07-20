﻿#region

using System.Collections.Generic;
using BuildingBlocks.Domain.DomainEvent;

#endregion

namespace BuildingBlocks.Domain
{
    public abstract class Entity
    {
        private List<IDomainEvent>? _domainEvents;
        
        public IReadOnlyCollection<IDomainEvent>? DomainEvents => _domainEvents?.AsReadOnly();

        public void ClearDomainEvents() => _domainEvents?.Clear();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            _domainEvents ??= new List<IDomainEvent>();

            _domainEvents.Add(domainEvent);
        }
    }
}