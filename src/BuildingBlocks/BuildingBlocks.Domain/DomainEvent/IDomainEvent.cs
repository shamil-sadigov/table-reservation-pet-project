using System;
using MediatR;

namespace BuildingBlocks.Domain.DomainEvent
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}