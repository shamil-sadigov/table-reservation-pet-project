#region

using Microsoft.EntityFrameworkCore;
using Reservations.Domain.ReservationRequestRejections;
using Reservations.Domain.ReservationRequests;
using Reservations.Domain.Reservations;

#endregion

namespace Reservations.Infrastructure.Contexts
{
    public class ApplicationContext : DbContext
    {
        // public ApplicationContext(DbContextOptions<ApplicationContext> ops)
        //     :base(ops)
        // {
        //     
        // }

        // Temp ctor. For migrations. Will be deleted
        public ApplicationContext()
        {
            
        }
        
        public DbSet<ReservationRequest> ReservationRequests { get; set; }
        
        public DbSet<ReservationRequestRejection> ReservationRequestRejections { get; set; }
        
        public DbSet<Reservation> Reservations { get; set; }

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