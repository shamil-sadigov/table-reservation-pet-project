#region

using System.Threading.Tasks;
using Reservation.Domain.Restaurants.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetById(RestaurantId restaurantId);
        Task AddAsync(Restaurant newRestaurant);
        void Update(Restaurant restaurant);
    }
}