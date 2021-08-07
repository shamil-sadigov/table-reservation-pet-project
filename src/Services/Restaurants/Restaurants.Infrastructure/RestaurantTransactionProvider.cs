#region

using System;
using System.Data.Common;
using EventBus.RabbitMq.Database;
using Microsoft.EntityFrameworkCore.Storage;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure
{
    public class RestaurantTransactionProvider : IDbTransactionProvider
    {
        private readonly RestaurantContext _context;

        public RestaurantTransactionProvider(RestaurantContext context)
        {
            _context = context;
        }

        public DbTransaction GetCurrentDbTransaction()
        {
            var transaction = _context.Database.CurrentTransaction;

            if (transaction is null) throw new InvalidOperationException("Restaurant transaction is not available");

            return transaction.GetDbTransaction();
        }
    }
}