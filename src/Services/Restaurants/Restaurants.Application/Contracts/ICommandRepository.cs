#region

using System;
using System.Threading.Tasks;

#endregion

namespace Restaurants.Application.Contracts
{
    public interface ICommandRepository
    {
        Task<Command?> GetByCorrelationIdAsync(Guid correlationId);
        Task SaveAsync(Command command);
    }
}