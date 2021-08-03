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
            builder.ToTable("Tables", schema: "restaurants");

            builder.HasKey(x => x.Id);

            builder.Property<TableState>("_state")
                .HasConversion<string>()
                .HasColumnName("State")
                .HasMaxLength(256);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new TableId(guid));

            builder.OwnsOne<NumberOfSeats>("NumberOfSeats", ops => 
                ops.Property(x => x.Value)
                    .HasColumnName("NumberOfSeats"));
            
            builder.HasOne<Restaurant>()
                .WithMany("_tables")
                .HasForeignKey("_restaurantId")
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property("_restaurantId")
                .HasColumnName("RestaurantId");
            
        }
    }
}