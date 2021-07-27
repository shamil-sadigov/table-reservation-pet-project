#region

using System;
using Ardalis.SmartEnum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors;

#endregion

namespace Reservation.Infrastructure.Databass.Configurations
{
    public class ReservationRequestEntityConfiguration : IEntityTypeConfiguration<ReservationRequest>
    {
        public void Configure(EntityTypeBuilder<ReservationRequest> builder)
        {
            builder.ToTable("ReservationRequests", schema: "reservation");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasConversion(x => x.Value, guid => new ReservationRequestId(guid));

            builder.HasOne<Table>()
                .WithMany()
                .HasForeignKey("_tableId");

            builder.HasOne<Visitor>()
                .WithMany()
                .HasForeignKey("_visitorId");

            builder.Property("_tableId")
                .HasColumnName("TableId");

            builder.Property("_visitorId")
                .HasColumnName("VisitorId");

            builder.Property<NumberOfSeats>("_numberOfRequestedSeats")
                .HasConversion(x => x.Value, value => CreateNumberOfSeatsFromValue(value))
                .HasColumnName("NumberOfRequestedSeats");


            builder.Property<ReservationRequestState>("_state")
                .HasColumnName("State")
                .HasConversion(
                    x => x.Name,
                    nameStr => ReservationRequestFromName(nameStr));

            builder.Property<DateTime>("_visitingDateTime")
                .HasColumnName("VisitingDateTime");
        }

        private static ReservationRequestState ReservationRequestFromName(string name)
        {
            try
            {
                return ReservationRequestState.FromName(name);
            }
            catch (SmartEnumNotFoundException catchException)
            {
                var exception = new DataCorruptionException(
                    "Retrieved ReservationRequestState name is corrupted. " +
                    "Unable to create ReservationRequestState", catchException);

                exception.Data.Add("ReservationRequestStateStr", name);

                throw exception;
            }
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