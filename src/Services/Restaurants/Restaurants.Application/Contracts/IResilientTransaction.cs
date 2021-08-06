#region

using System;
using System.Threading.Tasks;

#endregion

namespace Restaurants.Application.Contracts
{
    public interface IResilientTransaction
    {
        Task ExecuteAsync(Func<Task> func);
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}