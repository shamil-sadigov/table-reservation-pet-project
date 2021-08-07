#region

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using EventBus.RabbitMq.Database;

#endregion

namespace EventBus.RabbitMq.Abstractions
{
    public interface IIntegrationEventRepository
    {
        Task AddAsync(IntegrationEventEntry integrationEvent, DbTransaction transaction);

        Task UpdateAsync(IntegrationEventEntry integrationEvent);

        Task<List<IntegrationEventEntry>> GetUnpublishedEventsAsync(Guid correlationId);
    }
}