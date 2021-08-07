#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservations.Domain.ReservationRequests;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.ReservationRequests.ValueObjects.ReservationRequestStates;
using Reservations.Domain.Visitors;

#endregion

namespace Reservations.Infrastructure.Configurations
{
    public class ReservationRequestEntityConfiguration : IEntityTypeConfiguration<ReservationRequest>
    {
        public void Configure(EntityTypeBuilder<ReservationRequest> builder)
        {
            builder.ToTable("ReservationRequests", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .WithConversion();

            builder.Property<ReservationRequestState>("_state")
                .HasColumnName("State")
                .HasConversion(
                    x => x.Name,
                    nameStr => ConvertToReservationRequestState(nameStr))
                .IsConcurrencyToken();

            builder.Property<TableId>("_tableId")
                .HasColumnName("TableId")
                .WithConversion();

            builder.Property<RestaurantId>("_restaurantId")
                .HasColumnName("RestaurantId")
                .WithConversion();

            builder.Property<VisitorId>("_visitorId")
                .HasColumnName("VisitorId");

            builder.Property<DateTime>("_createdDateTime")
                .HasColumnName("CreatedDateTime")
                .WithUtcConversion();

            builder.Property<DateTime?>("_closedDateTime")
                .HasColumnName("ClosedDateTime")
                .WithUtcConversion();

            builder.Property<DateTime>("_visitingDateTime")
                .HasColumnName("VisitingDateTime")
                .WithUtcConversion();
            ;

            builder.HasOne<Visitor>()
                .WithMany()
                .HasForeignKey("_visitorId")
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static ReservationRequestState ConvertToReservationRequestState(string nameStr)
            => ReservationRequestState.FromName(nameStr);
    }
}