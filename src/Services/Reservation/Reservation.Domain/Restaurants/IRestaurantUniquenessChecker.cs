using System.Threading.Tasks;
using Reservation.Domain.Restaurants.ValueObjects;

namespace Reservation.Domain.Restaurants
{
    public interface IRestaurantUniquenessChecker
    {
        Task<bool> IsUniqueAsync(string restaurantName, RestaurantAddress restaurantAddress);
    }
}