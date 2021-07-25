#region

using System;
using Ardalis.SmartEnum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;

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

            builder.Property("_tableId")
                .HasColumnName("TableId");
            
            builder.Property<NumberOfSeats>("_numberOfRequestedSeats")
                .HasConversion(x=> x.Value, value => CreateNumberOfSeatsFromValue(value));
            
            builder.Property<ReservationRequestState>("_state")
                .HasColumnName("State")
                .HasConversion(
                    x => x.Name,
                    nameStr => ReservationRequestFromName(nameStr));

            builder.Property<VisitingTime>("_visitingTime")
                .HasColumnName("VisitingTime")
                .HasConversion(
                    x => new TimeSpan(x.Hours, x.Minutes, 0),
                    timeSpan => VisitingTimeFromTimeSpan(timeSpan));
        }

        private static VisitingTime VisitingTimeFromTimeSpan(TimeSpan timeSpan)
        {
            var result = VisitingTime.TryCreate((byte) timeSpan.Hours, (byte) timeSpan.Minutes);

            if (result.Succeeded)
                return result.Value!;

            var exception = new DataCorruptionException(
                "Retrieved TimeSpan is corrupted. " +
                "Unable to create VisitingTime");

            exception.Data.Add("TimeSpan", timeSpan);

            throw exception;
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