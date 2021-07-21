#region

using System;
using MediatR;

#endregion

namespace BuildingBlocks.Domain.DomainEvents
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }

        DateTime OccurredOn { get; }
    }
}