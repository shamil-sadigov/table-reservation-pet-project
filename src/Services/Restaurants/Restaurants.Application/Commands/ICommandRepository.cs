#region

using System;
using System.Threading.Tasks;

#endregion

namespace Restaurants.Application.Commands
{
    public interface ICommandRepository
    {
        Task<Command> GetAsync(Guid id);
        Task AddAsync(Command command);
    }
}