#region

using System;
using System.Threading.Tasks;
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

        public async Task<Command?> GetAsync(Guid id)
        {
            return await _context.ApplicationCommands.FindAsync(id);
        }

        public async Task SaveAsync(Command applicationCommand)
        {
            await _context.ApplicationCommands.AddAsync(applicationCommand);

            await _context.SaveChangesAsync();
        }
    }
}