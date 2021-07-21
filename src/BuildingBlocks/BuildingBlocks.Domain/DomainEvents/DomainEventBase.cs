#region

using System;

#endregion

namespace BuildingBlocks.Domain.DomainEvents
{
    public class DomainEventBase : IDomainEvent
    {
        public DomainEventBase()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }

        public Guid Id { get; }

        public DateTime OccurredOn { get; }
    }
}