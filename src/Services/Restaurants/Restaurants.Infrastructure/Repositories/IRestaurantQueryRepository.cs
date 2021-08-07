#region

using System.Threading.Tasks;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    public interface IRestaurantQueryRepository
    {
        Task<bool> ExistsAsync(string restaurantName, string restaurantAddress);
    }
}