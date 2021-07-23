using Microsoft.EntityFrameworkCore;
using Reservation.Domain.Restaurants;

namespace Reservation.Infrastructure.Databass
{
    public class ReservationContext:DbContext
    {
        
        // Uncomment when application layer is added
        
        // public ReservationDomainContext(DbContextOptions<ReservationDomainContext> ops)
        //     :base(ops)
        // {
        //     
        // }
        
        public DbSet<Restaurant> Restaurants { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // TODO: Extract to settings
            optionsBuilder.UseSqlServer("Data Source=darwin;Initial Catalog=ReservationApp;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}