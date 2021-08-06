namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish(IntegrationEvent integrationEvent);
        
        // TODO: Add additional methods to for event subscription
    }
}