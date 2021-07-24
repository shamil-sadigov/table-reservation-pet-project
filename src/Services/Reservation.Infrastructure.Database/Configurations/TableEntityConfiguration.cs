#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Infrastructure.Databass.Configurations
{
    public class TableEntityConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("Tables", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property<TableStatus>("_status")
                .HasConversion<string>()
                .HasColumnName("Status");

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new TableId(guid));

            builder.OwnsOne<NumberOfSeats>("NumberOfSeats",
                x => x.Property("_numberOfSeats")
                    .HasColumnName("NumberOfSeats"));

            builder.HasOne<Restaurant>()
                .WithMany()
                .HasForeignKey("_restaurantId");

            builder.Property("_restaurantId")
                .HasColumnName("RestaurantId");
        }
    }
}