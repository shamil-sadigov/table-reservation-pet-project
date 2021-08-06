#region

using Microsoft.EntityFrameworkCore;
using Restaurants.Application;
using Restaurants.Domain.Restaurants;

#endregion

namespace Restaurants.Infrastructure.Contexts
{
    // TODO: Add outgoing 'IntegrationEvents' set

    public class RestaurantContext : DbContext
    {
        public RestaurantContext(DbContextOptions<RestaurantContext> ops)
            : base(ops)
        {
        }

        public bool HasTransaction => Database.CurrentTransaction != null;

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}