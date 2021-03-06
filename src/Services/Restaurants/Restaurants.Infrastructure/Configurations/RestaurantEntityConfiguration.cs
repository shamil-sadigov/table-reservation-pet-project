#region

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Infrastructure.Configurations
{
    public class RestaurantEntityConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants", schema: "restaurants");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new RestaurantId(guid));

            builder.OwnsOne<RestaurantName>("_name", ops =>
                ops.Property(x => x.Value)
                    .HasColumnName("Name")
                    .HasMaxLength(256));

            builder.OwnsOne<RestaurantAddress>("_address", ops =>
                ops.Property(x => x.Value)
                    .HasColumnName("Address")
                    .HasMaxLength(256));

            builder.OwnsOne<RestaurantWorkingHours>("_workingHours", x =>
            {
                x.Property(ra => ra.StartTime)
                    .HasColumnName("StartWorkingAt")
                    .HasPrecision(0, 0);

                x.Property(ra => ra.FinishTime)
                    .HasColumnName("FinishWorkingAt")
                    .HasPrecision(0, 0);
            });

            builder.Navigation("_name").IsRequired();
            builder.Navigation("_address").IsRequired();
            builder.Navigation("_workingHours").IsRequired();
        }
    }
}