#region

using Microsoft.EntityFrameworkCore;
using Restaurants.Application;

#endregion

namespace Restaurants.Infrastructure.Contexts
{
    // TODO: Add configuration for IncomingCommand
    public class ApplicationContext : DbContext
    {
        // TODO: Add outgoing 'IntegrationEvents' set

        public ApplicationContext(DbContextOptions<ApplicationContext> ops)
            : base(ops)
        {
        }

        public DbSet<Command> Commands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            => modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}