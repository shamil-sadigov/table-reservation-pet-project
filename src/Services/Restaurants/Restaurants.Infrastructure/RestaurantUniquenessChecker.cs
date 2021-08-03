#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Infrastructure.Repositories;

#endregion

namespace Restaurants.Infrastructure
{
    public class RestaurantUniquenessChecker : IRestaurantUniquenessChecker
    {
        private readonly IRestaurantQueryRepository _repository;

        public RestaurantUniquenessChecker(IRestaurantQueryRepository repository)
            => _repository = repository;

        public async Task<bool> IsUniqueAsync(RestaurantName restaurantName, RestaurantAddress restaurantAddress)
        {
            return !await _repository.ExistsAsync(restaurantName.Value, restaurantAddress.Value);
        }
    }
}