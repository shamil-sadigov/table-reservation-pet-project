#region

using System.Net.Sockets;
using EventBus.Abstractions;

#endregion

namespace Restaurants.Api.IntegrationTests.EventBus
{
    public class UnAvailableEventBus : IEventBus
    {
        public void Publish(IntegrationEvent integrationEvent)
        {
            throw new SocketException();
        }
    }
}