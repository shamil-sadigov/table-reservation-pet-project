#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventBus.RabbitMq.Abstractions;
using Microsoft.EntityFrameworkCore;

#endregion

namespace EventBus.RabbitMq.Database
{
    public sealed class IntegrationEventRepository : IIntegrationEventRepository
    {
        private readonly IntegrationEventContext _context;

        public IntegrationEventRepository(IDbConnectionProvider connectionProvider)
        {
            var dbConnection = connectionProvider.GetDbConnection();

            // TODO: Move this configuration to Startup
            var dbContextOptions = new DbContextOptionsBuilder<IntegrationEventContext>()
                .UseSqlServer(dbConnection)
                .Options;

            _context = new IntegrationEventContext(dbContextOptions);
        }

        public async Task AddAsync(IntegrationEventEntry integrationEvent)
            => await _context.IntegrationEvents.AddAsync(integrationEvent);

        public void Update(IntegrationEventEntry integrationEvent)
            => _context.Entry(integrationEvent).CurrentValues.SetValues(integrationEvent);

        public async Task<List<IntegrationEventEntry>> GetUnpublishedEventsAsync(Guid correlationId)
            => await _context.IntegrationEvents
                .Where(x => x.CorrelationId == correlationId)
                .Where(x => x.State == IntegrationEventState.Unpublished)
                .ToListAsync();
    }
}