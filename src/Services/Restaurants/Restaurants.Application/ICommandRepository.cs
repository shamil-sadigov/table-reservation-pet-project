#region

using System;
using System.Threading.Tasks;

#endregion

namespace Restaurants.Application
{
    public interface ICommandRepository
    {
        Task<ApplicationCommand> GetAsync(Guid id);
        Task SaveAsync(ApplicationCommand applicationCommand);
    }
}