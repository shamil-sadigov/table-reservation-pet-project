using System;
using System.Threading.Tasks;

namespace Restaurants.Application
{
    public interface IResilientTransaction
    {
        Task ExecuteAsync(Func<Task> func);
        Task<T> ExecuteAsync<T>(Func<Task<T>> func);
    }
}