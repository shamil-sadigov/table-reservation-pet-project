using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Infrastructure.Databass.Configurations
{
    public class ReservationRequestRejectionTypeConfiguration : IEntityTypeConfiguration<ReservationRequestRejection>
    {
        public void Configure(EntityTypeBuilder<ReservationRequestRejection> builder)
        {
            builder.ToTable("ReservationRequestRejections", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new ReservationRequestRejectionId(guid));
            
            builder.Property<AdministratorId>("_rejectedByAdministratorId")
                .HasColumnName("RejectedByAdministratorId")
                .HasConversion(x => x.Value, guid => new AdministratorId(guid));

            builder.Property<DateTime>("_rejectionDateTime")
                .HasColumnName("RejectionDateTime");
            
            builder.Property<string>("_reason")
                .HasColumnName("Reason");
            
            builder.HasOne<ReservationRequest>()
                .WithOne()
                .HasForeignKey<ReservationRequestRejection>("_reservationRequestId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property<ReservationRequestId>("_reservationRequestId")
                .HasColumnName("ReservationRequestId");
        }
    }
}