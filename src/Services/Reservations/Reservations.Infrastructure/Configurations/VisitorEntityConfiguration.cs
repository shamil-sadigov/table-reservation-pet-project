#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Domain.Visitors;
using Restaurants.Domain.Visitors.ValueObjects;

#endregion

namespace Restaurants.Infrastructure.Configurations
{
    public class VisitorEntityConfiguration : IEntityTypeConfiguration<Visitor>
    {
        public void Configure(EntityTypeBuilder<Visitor> builder)
        {
            builder.ToTable("Visitors", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new VisitorId(guid));
        }
    }
}