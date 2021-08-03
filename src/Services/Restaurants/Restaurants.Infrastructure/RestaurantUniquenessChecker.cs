#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;

#endregion

namespace Restaurants.Infrastructure
{
    public class RestaurantUniquenessChecker : IRestaurantUniquenessChecker
    {
        private readonly IRestaurantQueryRepository _repository;

        public RestaurantUniquenessChecker(IRestaurantQueryRepository repository)
            => _repository = repository;

        public async Task<bool> IsUniqueAsync(string restaurantName, string restaurantAddress)
            => !await _repository.ExistsAsync(restaurantName, restaurantAddress);
    }
}