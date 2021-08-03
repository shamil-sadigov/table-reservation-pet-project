#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetById(RestaurantId restaurantId);
        Task AddAsync(Restaurant newRestaurant);
        void Update(Restaurant restaurant);
    }
}