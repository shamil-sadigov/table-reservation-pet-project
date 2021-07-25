using System;
using System.Threading.Tasks;

namespace Reservation.Application
{
    public interface ICommandRepository
    {
        Task<Command> GetAsync(Guid id);
        Task SaveAsync(Command command);
    }
}