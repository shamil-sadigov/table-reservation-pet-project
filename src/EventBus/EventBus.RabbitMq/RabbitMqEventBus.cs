#region

using System;
using EventBus.Abstractions;

#endregion

namespace EventBus.RabbitMq
{
    // TODO: Implement! + Add mechanism for subscriptions
    public class RabbitMqEventBus : IEventBus
    {
        public void Publish(IntegrationEvent integrationEvent)
        {
            throw new NotImplementedException();
        }
    }
}