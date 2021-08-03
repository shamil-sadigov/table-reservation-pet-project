#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurants.Infrastructure.Configurations
{
    public class TableEntityConfiguration : IEntityTypeConfiguration<Table>
    {
        public void Configure(EntityTypeBuilder<Table> builder)
        {
            builder.ToTable("Tables", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property<TableState>("_state")
                .HasConversion<string>()
                .HasColumnName("State");

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new TableId(guid));

            builder.Property<NumberOfSeats>("NumberOfSeats")
                .HasConversion(x => x.Value, value => CreateNumberOfSeatsFromValue(value));

            builder.HasOne<Restaurant>()
                .WithMany("_tables")
                .HasForeignKey("_restaurantId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property("_restaurantId")
                .HasColumnName("RestaurantId");
        }

        private static NumberOfSeats CreateNumberOfSeatsFromValue(byte value)
        {
            var result = NumberOfSeats.TryCreate(value);

            if (result.Succeeded)
                return result.Value!;

            var exception = new DataCorruptionException(
                "Retrieved byte value is corrupted. " +
                "Unable to create NumberOfSeats");

            exception.Data.Add("value", value);

            throw exception;
        }
    }
}