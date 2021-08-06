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
        // public ApplicationContext(DbContextOptions<ApplicationContext> ops)
        //     :base(ops)
        // {
        //     
        // }

        // Temp ctor. For migrations. Will be deleted
        public RestaurantContext()
        {
            
        }

        public bool HasTransaction => Database.CurrentTransaction != null;
        
        public DbSet<Restaurant> Restaurants { get; set; }
        
        public DbSet<Command> ApplicationCommands { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Extract to settings
            // Uncomment if you want to add migrations
            optionsBuilder.UseSqlServer("Data Source=darwin;Initial Catalog=ReservationApp;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}