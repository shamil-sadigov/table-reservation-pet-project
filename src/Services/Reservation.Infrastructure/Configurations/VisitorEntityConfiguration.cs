#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Infrastructure.Databass.Configurations
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