using EventBus.Abstractions;

namespace Restaurants.Api.IntegrationTests
{
    public class AvailableEventBus:IEventBus
    {
        public void Publish(IntegrationEvent integrationEvent)
        {
            // successfully published
        }
    }
}