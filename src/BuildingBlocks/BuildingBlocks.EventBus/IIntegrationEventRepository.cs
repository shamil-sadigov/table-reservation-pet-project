using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingBlocks.EventBus
{
    public interface IIntegrationEventRepository
    {
        Task AddAsync(IntegrationEventEntry integrationEvent);
        
        void Update(IntegrationEventEntry integrationEvent);
        
        Task<List<IntegrationEventEntry>> GetUnpublishedEventsAsync(Guid correlationId);
    }
}