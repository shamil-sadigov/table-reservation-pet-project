#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.Contracts
{
    public interface IRestaurantChecker
    {
        Task<bool> ExistsAsync(RestaurantName restaurantName, RestaurantAddress restaurantAddress);
    }
}