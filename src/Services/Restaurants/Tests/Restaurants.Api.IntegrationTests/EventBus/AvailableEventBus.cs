#region

using EventBus.Abstractions;

#endregion

namespace Restaurants.Api.IntegrationTests.EventBus
{
    public class AvailableEventBus : IEventBus
    {
        public void Publish(IntegrationEvent integrationEvent)
        {
            // successfully published
        }
    }
}