#region

using System.Threading.Tasks;
using Dapper;
using Reservation.Application.Commands.Restaurants;

#endregion

namespace Reservation.Infrastructure.Databass.Repositories
{
    public class RestaurantQueryRepository : IRestaurantQueryRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public RestaurantQueryRepository(ISqlConnectionFactory sqlConnectionFactory)
            => _sqlConnectionFactory = sqlConnectionFactory;

        public async Task<bool> ExistsAsync(string restaurantName, string restaurantAddress)
        {
            var connection = _sqlConnectionFactory.GetOrCreateConnection();

            var result = await connection.QuerySingleOrDefaultAsync(
                "SELECT [Id] FROM reservation.Restaurants " +
                "WHERE [Address]=@Address AND [Name]=@Name", new
                {
                    Name = restaurantName,
                    Address = restaurantAddress
                });

            return result is not null;
        }
    }
}