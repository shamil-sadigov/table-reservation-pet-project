#region

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Infrastructure.Repositories
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantContext _context;

        public RestaurantRepository(RestaurantContext context)
            => _context = context;


        public async Task AddAsync(Restaurant newRestaurant)
            => await _context.AddAsync(newRestaurant);

        public void Update(Restaurant restaurant)
            => _context.Entry(restaurant).CurrentValues.SetValues(restaurant);

        public async Task<Restaurant?> GetAsync(RestaurantId restaurantId)
        {
            return await _context.Restaurants
                .Include("_tables")
                .FirstOrDefaultAsync(x => x.Id == restaurantId);
        }
    }
}