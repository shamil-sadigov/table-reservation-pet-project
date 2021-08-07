#region

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using EventBus.RabbitMq.Abstractions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

#endregion

namespace EventBus.RabbitMq.Database
{
    public sealed class IntegrationEventRepository : IIntegrationEventRepository
    {
        private readonly IntegrationEventContext _context;

        public IntegrationEventRepository(IntegrationEventContext context)
        {
            _context = context;
        }

        public async Task AddAsync(IntegrationEventEntry integrationEvent, DbTransaction transaction)
        {
            await _context.Database.UseTransactionAsync(transaction);
            await _context.IntegrationEvents.AddAsync(integrationEvent);

            await _context.SaveChangesAsync();

            var connectionString = _context.Database.GetConnectionString();
            var databaseCurrentTransaction = _context.Database.CurrentTransaction;
        }

        public async Task UpdateAsync(IntegrationEventEntry integrationEvent)
        {
            _context.Entry(integrationEvent).CurrentValues.SetValues(integrationEvent);
            await _context.SaveChangesAsync();
        }
        
        public async Task<List<IntegrationEventEntry>> GetUnpublishedEventsAsync(Guid correlationId)
            => await _context.IntegrationEvents
                .Where(x => x.CorrelationId == correlationId)
                .Where(x => x.State == IntegrationEventState.Unpublished)
                .ToListAsync();
    }
}