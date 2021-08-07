using EventBus.RabbitMq;
using EventBus.RabbitMq.Database;
using FluentAssertions;
using Restaurants.Application;

namespace Restaurants.Api.IntegrationTests.Helpers
{
    // TODO: Extract this extension to shared project, because it's probably going to be reused
    // in other services
    
    public static class IntegrationEventExtensions
    {
        public static void ShouldBeCausedBy(this IntegrationEventEntry integrationEvent, Command causedByCommand)
        {
            integrationEvent.CausationId
                .Should()
                .Be(causedByCommand.CommandId, $"Expected integrationEvent {integrationEvent.EventId} to be cause " +
                                               $"by command {causedByCommand.CommandId}");
        }

        public static void ShouldBePublished(this IntegrationEventEntry integrationEvent)
        {
            integrationEvent.State
                .Should()
                .Be(IntegrationEventState.Published, $"Expected integrationEvent {integrationEvent.EventId} to be published " 
                                                     +$"but it turn out to be {integrationEvent.State}");
        }
        
        public static void ShouldBeFailedToPublish(this IntegrationEventEntry integrationEvent)
        {
            integrationEvent.State
                .Should()
                .Be(IntegrationEventState.Failed, $"Expected integrationEvent {integrationEvent.EventId} to be published " 
                                                     +$"but it turn out to be {integrationEvent.State}");
        }
    }
}