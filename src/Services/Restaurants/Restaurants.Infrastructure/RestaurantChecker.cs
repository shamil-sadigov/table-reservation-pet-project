#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Infrastructure.Repositories;

#endregion

namespace Restaurants.Infrastructure
{
    public class RestaurantChecker : IRestaurantChecker
    {
        private readonly IRestaurantQueryRepository _repository;

        public RestaurantChecker(IRestaurantQueryRepository repository)
            => _repository = repository;

        public async Task<bool> ExistsAsync(RestaurantName restaurantName, RestaurantAddress restaurantAddress)
        {
            return await _repository.ExistsAsync(restaurantName.Value, restaurantAddress.Value);
        }
    }
}