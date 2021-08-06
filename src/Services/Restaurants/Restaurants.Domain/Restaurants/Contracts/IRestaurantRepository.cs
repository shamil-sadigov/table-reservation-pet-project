#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.Contracts
{
    public interface IRestaurantRepository
    {
        Task<Restaurant?> GetAsync(RestaurantId restaurantId);
        Task AddAsync(Restaurant newRestaurant);
        void Update(Restaurant restaurant);
    }
}