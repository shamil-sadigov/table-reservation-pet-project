#region

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.ValueObjects;

#endregion

namespace Reservation.Infrastructure.Databass.Configurations
{
    public class RestaurantEntityConfiguration : IEntityTypeConfiguration<Restaurant>
    {
        public void Configure(EntityTypeBuilder<Restaurant> builder)
        {
            builder.ToTable("Restaurants", schema: "reservation");

            builder.HasKey(x => x.Id);
            
            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new RestaurantId(guid));
            
            builder.Property<string>("_name")
                .HasColumnName("Name");
            
            builder.OwnsOne<RestaurantAddress>("_address", x =>
            {
                x.Property(ra => ra.Value)
                    .HasColumnName("Address");
            });
            
            builder.OwnsOne<RestaurantWorkingHours>("_workingHours", x =>
            {
                x.Property(ra => ra.StartTime)
                    .HasColumnName("StartWorkingAt");
                
                x.Property(ra => ra.FinishTime)
                    .HasColumnName("FinishWorkingAt");
            });

            builder.Navigation("_address")
                .IsRequired();
            
            builder.Navigation("_workingHours")
                .IsRequired();
        }
    }
}