using System;
using System.Threading.Tasks;
using Reservation.Application;

namespace Reservation.Infrastructure.Databass.Repositories
{
    public class CommandRepository:ICommandRepository
    {
        private readonly ApplicationContext _context;

        public CommandRepository(ApplicationContext context) 
            => _context = context;

        public async Task<Command> GetAsync(Guid id)
        {
            return  await _context.Commands.FindAsync(id);
        }

        public async Task SaveAsync(Command command)
        {
            await _context.Commands.AddAsync(command);

            await _context.SaveChangesAsync();
        }
    }
}