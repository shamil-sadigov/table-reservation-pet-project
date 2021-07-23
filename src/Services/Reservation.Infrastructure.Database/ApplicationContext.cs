using Microsoft.EntityFrameworkCore;

namespace Reservation.Infrastructure.Databass
{
    public class ApplicationContext:DbContext
    {
        // TODO: Add 'Commands' set to check idempotency
        
        // TODO: Add outgoing 'IntegrationEvents' set
        
        public ApplicationContext(DbContextOptions<ApplicationContext> ops)
            :base(ops)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}