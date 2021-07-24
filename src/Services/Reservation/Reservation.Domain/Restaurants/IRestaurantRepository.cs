using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.Restaurants.ValueObjects;

namespace Reservation.Domain.Restaurants
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> GetById(RestaurantId restaurantId);

        Task Add(Restaurant newRestaurant);
    }
}