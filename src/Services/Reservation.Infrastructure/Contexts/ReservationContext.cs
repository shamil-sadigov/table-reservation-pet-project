using Microsoft.EntityFrameworkCore;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.Restaurants;

namespace Reservation.Infrastructure.Databass.Contexts
{
    public class ReservationContext:DbContext
    {
        // Uncomment when application layer is added
        // public ReservationContext(DbContextOptions<ReservationContext> ops)
        //     :base(ops)
        // {
        //     
        // }

        public ReservationContext()
        {
            
        }
        
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<ReservationRequest> ReservationRequests { get; set; }
        
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