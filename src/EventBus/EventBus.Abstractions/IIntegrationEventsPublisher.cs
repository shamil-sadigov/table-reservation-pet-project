#region

using System;
using System.Threading.Tasks;

#endregion

namespace EventBus.Abstractions
{
    public interface IIntegrationEventsPublisher
    {
        /// <summary>
        ///     Adds event to storage as unpublished. And will be published later on
        ///     invocation of <see cref="PublishEventsAsync" />
        /// </summary>
        Task RegisterEventAsync(IntegrationEvent @event);

        /// <summary>
        ///     Takes unpublished event from storage and publishes to event bus
        /// </summary>
        Task PublishEventsAsync(Guid correlationId);
    }
}