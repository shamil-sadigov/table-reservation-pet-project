#region

using System.Threading.Tasks;
using Dapper;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    // Repository for Query part of CQRS
    // Additional methods will be added
    public class RestaurantQueryRepository : IRestaurantQueryRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public RestaurantQueryRepository(ISqlConnectionFactory sqlConnectionFactory)
            => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<bool> ExistsAsync(string restaurantName, string restaurantAddress)
        {
            var connection = _sqlConnectionFactory.GetOrCreateConnection();

            var result = await connection.QuerySingleOrDefaultAsync(
                "SELECT [Id] FROM restaurants.Restaurants " +
                "WHERE [Address]=@Address AND [Name]=@Name", new
                {
                    Name = restaurantName,
                    Address = restaurantAddress
                });

            return result is not null;
        }
    }
}