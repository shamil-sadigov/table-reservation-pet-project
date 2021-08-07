#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequestRejections;
using Reservations.Domain.ReservationRequestRejections.ValueObjects;
using Reservations.Domain.ReservationRequests;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Infrastructure.Configurations
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
                .WithConversion();

            builder.Property<DateTime>("_rejectionDateTime")
                .HasColumnName("RejectionDateTime")
                .WithUtcConversion();

            builder.Property<RejectionReason>("_reason")
                .HasColumnName("Reason")
                .WithConversion();

            builder.Property<ReservationRequestId>("_reservationRequestId")
                .HasColumnName("ReservationRequestId");

            builder.HasOne<ReservationRequest>()
                .WithOne()
                .HasForeignKey<ReservationRequestRejection>("_reservationRequestId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}