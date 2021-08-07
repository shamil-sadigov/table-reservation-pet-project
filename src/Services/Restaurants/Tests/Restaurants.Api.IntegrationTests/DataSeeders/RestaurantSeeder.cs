using Microsoft.EntityFrameworkCore;

namespace Restaurants.Api.IntegrationTests.DataSeeders
{
    public class RestaurantSeeder:IDataSeeder
    {
        private readonly Domain.Restaurants.Restaurant[] _restaurant;

        public RestaurantSeeder(params Domain.Restaurants.Restaurant[] restaurant)
        {
            _restaurant = restaurant;
        }
        
        public void Seed(DbContext context)
        {
            context.Set<Domain.Restaurants.Restaurant>().AddRange(_restaurant);
            context.SaveChanges();
        }
    }
}