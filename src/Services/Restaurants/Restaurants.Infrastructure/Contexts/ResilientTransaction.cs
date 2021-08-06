using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurants.Application;

namespace Restaurants.Infrastructure.Contexts
{
    public class ResilientTransaction:IResilientTransaction
    {
        private readonly RestaurantContext _context;

        public ResilientTransaction(RestaurantContext context)
        {
            _context = context;
        }
        
        public async Task ExecuteAsync(Func<Task> func)
        {
            if (_context.HasTransaction)
                throw new InvalidOperationException("Transaction has been already began");
            
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                
                await func();
                
                await transaction.CommitAsync();
            });
        }

        public async Task<T> ExecuteAsync<T>(Func<Task<T>> func)
        {
            if (_context.HasTransaction)
                throw new InvalidOperationException("Transaction has been already began");
            
            var executionStrategy = _context.Database.CreateExecutionStrategy();

            return await executionStrategy.ExecuteAsync(async () =>
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                
                var result = await func();
                
                await transaction.CommitAsync();
                
                return result;
            });
        }
    }
}