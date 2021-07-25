using System.Threading.Tasks;

namespace Reservation.Domain.Restaurants
{
    public interface IRestaurantUniquenessChecker
    {
        Task<bool> IsUniqueAsync(string restaurantName, string restaurantAddress);
    }
}