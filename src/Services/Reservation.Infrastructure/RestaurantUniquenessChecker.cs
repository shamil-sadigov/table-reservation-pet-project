#region

using System.Threading.Tasks;
using Reservation.Application.Commands.Restaurants;
using Reservation.Domain.Restaurants;

#endregion

namespace Reservation.Infrastructure.Databass
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