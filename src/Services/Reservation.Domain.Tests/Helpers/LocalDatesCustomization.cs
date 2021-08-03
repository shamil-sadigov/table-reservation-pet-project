using System;
using AutoFixture;
using BuildingBlocks.Tests.Shared;
using Reservations.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.Tests
{
    public class LocalDatesCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            RegisterRejectionReason(fixture);

            RegisterTableId(fixture);
        }

        private static void RegisterTableId(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var result = TableId.TryCreate(fixture.Create<string>());

                result.ThrowIfNotSuccessful();

                return result.Value!;
            });
        }

        private static void RegisterRejectionReason(IFixture fixture)
        {
            fixture.Register(() =>
            {
                var result = RejectionReason.TryCreate(fixture.Create<string>());

                result.ThrowIfNotSuccessful();

                return result.Value!;
            });
        }
    }
}