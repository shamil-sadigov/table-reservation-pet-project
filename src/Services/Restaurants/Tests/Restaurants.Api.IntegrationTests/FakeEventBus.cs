using EventBus.Abstractions;

namespace Restaurants.Api.IntegrationTests
{
    public class FakeEventBus:IEventBus
    {
        public void Publish(IntegrationEvent integrationEvent)
        {
            // suppose we published
        }
    }
}