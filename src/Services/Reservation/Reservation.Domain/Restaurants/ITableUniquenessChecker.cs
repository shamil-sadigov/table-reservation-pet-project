using System.Threading.Tasks;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

namespace Reservation.Domain.Restaurants
{
    public interface ITableUniquenessChecker
    {
        Task<bool> IsUniqueAsync(RestaurantId restaurantId, TableId tableId);
    }
}