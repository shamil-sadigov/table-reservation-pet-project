using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Visitors;

namespace Reservations.Infrastructure.Configurations
{
    public class VisitorEntityConfiguration : IEntityTypeConfiguration<Visitor>
    {
        public void Configure(EntityTypeBuilder<Visitor> builder)
        {
            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new VisitorId(guid));
        }
    }
}