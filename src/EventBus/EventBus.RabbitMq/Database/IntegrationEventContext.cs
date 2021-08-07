#region

using Microsoft.EntityFrameworkCore;

#endregion

namespace EventBus.RabbitMq.Database
{
    public sealed class IntegrationEventContext : DbContext
    {
        public IntegrationEventContext(DbContextOptions<IntegrationEventContext> options)
            : base(options)
        {
            
        }
        
        public DbSet<IntegrationEventEntry> IntegrationEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IntegrationEventEntry>(model =>
            {
                model.HasKey(x => x.EventId);

                // model.Property(x => x.CorrelationId);

                model.HasIndex(x => x.CorrelationId);

                model.Property(x => x.CausationId);

                model.Property(x => x.CreationDate);

                model.Property(x => x.EventType)
                    .HasMaxLength(256);
                
                model.Property(x => x.EventContent)
                    .IsRequired();
                
                model.Property(x => x.State)
                    .HasConversion<string>()
                    .HasMaxLength(256);
            });
        }
    }
}