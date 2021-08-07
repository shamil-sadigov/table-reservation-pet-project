#region

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequests;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Reservations;
using Reservations.Domain.Visitors;

#endregion

namespace Reservations.Infrastructure.Configurations
{
    public class ReservationEntityTypeConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .WithConversion();

            builder.Property<AdministratorId>("_approvedByAdministratorId")
                .HasColumnName("ApprovedByAdministratorId")
                .WithConversion();

            builder.Property<TableId>("_tableId")
                .HasColumnName("TableId")
                .WithConversion();

            builder.Property<VisitorId>("_visitorId")
                .HasColumnName("VisitorId");

            builder.Property<DateTime>("_approvedDateTime")
                .HasColumnName("ApprovedDateTime")
                .WithUtcConversion();

            builder.Property<ReservationRequestId>("_reservationRequestId")
                .HasColumnName("ReservationRequestId");

            builder.HasOne<ReservationRequest>()
                .WithOne()
                .HasForeignKey<Reservation>("_reservationRequestId")
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne<Visitor>()
                .WithMany()
                .HasForeignKey("_visitorId")
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}