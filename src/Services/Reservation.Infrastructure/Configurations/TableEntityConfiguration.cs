﻿#region

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

            builder.Property<NumberOfSeats>("NumberOfSeats")
                .HasConversion(x=> x.Value, value => CreateNumberOfSeatsFromValue(value));
            
            builder.HasOne<Restaurant>()
                .WithMany("_tables")
                .HasForeignKey("_restaurantId");

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