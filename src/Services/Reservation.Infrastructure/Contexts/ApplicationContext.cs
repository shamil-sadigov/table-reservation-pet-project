using Microsoft.EntityFrameworkCore;
using Reservation.Application;

namespace Reservation.Infrastructure.Databass.Contexts
{
    // TODO: Add configuration for IncomingCommand
    public class ApplicationContext:DbContext
    {
        public DbSet<Command> Commands { get; set; }
        
        // TODO: Add outgoing 'IntegrationEvents' set
        
        public ApplicationContext(DbContextOptions<ApplicationContext> ops)
            :base(ops)
        {
            
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}