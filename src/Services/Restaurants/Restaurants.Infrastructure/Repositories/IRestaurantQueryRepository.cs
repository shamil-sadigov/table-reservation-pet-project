using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Repositories
{
    public interface IRestaurantQueryRepository
    {
        Task<bool> ExistsAsync(string restaurantName, string restaurantAddress);
    }
}