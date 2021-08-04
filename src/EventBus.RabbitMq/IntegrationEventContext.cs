using BuildingBlocks.EventBus;
using Microsoft.EntityFrameworkCore;

namespace EventBus.RabbitMq
{
    public sealed class IntegrationEventContext:DbContext
    {
        public IntegrationEventContext(DbContextOptions<IntegrationEventContext> options)
            :base(options)
        {
            
        }
        
        public DbSet<IntegrationEventEntry> IntegrationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationEventEntry>(model =>
            {
                model.HasKey(x => x.EventId);

                model.Property(x => x.State)
                    .HasConversion<string>();

                model.Property(x => x.EventContent)
                    .IsRequired();
            });
        }
    }
}