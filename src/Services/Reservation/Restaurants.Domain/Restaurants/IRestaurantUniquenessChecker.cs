#region

using System.Threading.Tasks;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants
{
    public interface IRestaurantUniquenessChecker
    {
        Task<bool> IsUniqueAsync(string restaurantName, RestaurantAddress restaurantAddress);
    }
}