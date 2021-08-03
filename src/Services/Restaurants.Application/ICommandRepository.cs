#region

using System;
using System.Threading.Tasks;

#endregion

namespace Restaurants.Application
{
    public interface ICommandRepository
    {
        Task<Command> GetAsync(Guid id);
        Task SaveAsync(Command command);
    }
}