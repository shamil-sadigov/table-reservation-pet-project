#region

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Infrastructure.Databass.Contexts;

#endregion

namespace Reservation.Infrastructure.Databass.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly ReservationContext _context;

        public RestaurantRepository(ReservationContext context)
            => _context = context;


        public async Task AddAsync(Restaurant newRestaurant)
            => await _context.AddAsync(newRestaurant);

        public void Update(Restaurant restaurant)
            => _context.Entry(restaurant).CurrentValues.SetValues(restaurant);

        public async Task<Restaurant?> GetById(RestaurantId restaurantId)
        {
            return await _context.Restaurants
                .Include("_tables")
                .FirstOrDefaultAsync(x => x.Id == restaurantId);
        }
    }
}