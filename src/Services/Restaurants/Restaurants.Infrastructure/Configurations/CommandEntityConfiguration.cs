#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Application;

#endregion

namespace Restaurants.Infrastructure.Configurations
{
    public class CommandEntityConfiguration : IEntityTypeConfiguration<Command>
    {
        public void Configure(EntityTypeBuilder<Command> builder)
        {
            builder.HasKey(x => x.CommandId);

            builder.Property(x => x.CorrelationId);

            builder.HasIndex(x => x.CorrelationId);

            builder.Property(x => x.CommandType)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(x => x.CausationId);

            builder.Property(x => x.CreationDate);
        }
    }
}