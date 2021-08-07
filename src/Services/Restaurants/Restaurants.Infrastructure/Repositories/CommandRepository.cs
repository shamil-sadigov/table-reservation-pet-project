#region

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application;
using Restaurants.Application.Contracts;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly RestaurantContext _context;

        public CommandRepository(RestaurantContext context)
            => _context = context;

        public async Task<Command?> GetByCorrelationIdAsync(Guid correlationId)
        {
            return await _context.Commands
                .SingleOrDefaultAsync(x => x.CorrelationId == correlationId);
        }

        public async Task SaveAsync(Command applicationCommand)
        {
            await _context.Commands.AddAsync(applicationCommand);

            await _context.SaveChangesAsync();
        }
    }
}