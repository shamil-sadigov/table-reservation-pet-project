using System.Data.Common;
using EventBus.RabbitMq.Abstractions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Infrastructure.Contexts;

namespace Restaurants.Infrastructure
{
    public class RestaurantDbConnectionProvider:IDbConnectionProvider
    {
        private readonly RestaurantContext _dbContext;

        public RestaurantDbConnectionProvider(RestaurantContext dbContext) 
            => _dbContext = dbContext;

        public DbConnection GetDbConnection() 
            => _dbContext.Database.GetDbConnection();
    }
}