#region

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventBus.RabbitMq.Database;

#endregion

namespace EventBus.RabbitMq.Abstractions
{
    public interface IIntegrationEventRepository
    {
        Task AddAsync(IntegrationEventEntry integrationEvent);

        void Update(IntegrationEventEntry integrationEvent);

        Task<List<IntegrationEventEntry>> GetUnpublishedEventsAsync(Guid correlationId);
    }
}