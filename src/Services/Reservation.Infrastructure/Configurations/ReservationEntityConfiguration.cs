#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservation.Infrastructure.Databass.Configurations
{
    public class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Domain.ReservationRequests.Reservation>
    {
        public void Configure(EntityTypeBuilder<Domain.ReservationRequests.Reservation> builder)
        {
            builder.ToTable("Reservations", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new ReservationId(guid));
            
            builder.Property<AdministratorId>("_approvedByAdministratorId")
                .HasColumnName("ApprovedByAdministratorId")
                .HasConversion(x => x.Value, guid => new AdministratorId(guid));
            
            builder.Property<DateTime>("_approvedDateTime")
                .HasColumnName("ApprovedDateTime");

            builder.HasOne<ReservationRequest>()
                .WithOne()
                .HasForeignKey<Domain.ReservationRequests.Reservation>("_reservationRequestId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<ReservationRequestId>("_reservationRequestId")
                .HasColumnName("ReservationRequestId");
        }
    }
}