#region

using System;
using System.Threading.Tasks;
using Restaurants.Application;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly ApplicationContext _context;

        public CommandRepository(ApplicationContext context)
            => _context = context;

        public async Task<ApplicationCommand> GetAsync(Guid id)
        {
            return await _context.Commands.FindAsync(id);
        }

        public async Task SaveAsync(ApplicationCommand applicationCommand)
        {
            await _context.Commands.AddAsync(applicationCommand);

            await _context.SaveChangesAsync();
        }
    }
}