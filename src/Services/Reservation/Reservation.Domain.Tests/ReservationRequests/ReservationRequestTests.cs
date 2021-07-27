#region

using System;
using System.Threading.Tasks;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Tests.Helpers;
using Reservation.Domain.Visitors.ValueObjects;
using Xunit;

#endregion

namespace Reservation.Domain.Tests.ReservationRequests
{
    public class ReservationRequestTests
    {
        // TODO: Move all test below to separate test class

        [Theory]
        [InlineData(15, 00, 15, 15)]
        [InlineData(10, 15, 11, 15)]
        [InlineData(13, 15, 13, 20)]
        public async Task Cannot_approve_pending_reservation_request_when_visiting_date_has_expired(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;

            var pendingReservationRequest = await CreateReservationRequest(visitingTime);

            var administratorId = new AdministratorId(Guid.NewGuid());

            var approvalDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            // Act
            var result = pendingReservationRequest.TryApprove(
                administratorId,
                approvalDateTime,
                SystemTimeStub.Instance);

            // Assert
            result.ShouldFail();
        }

        // TODO: Extract to builder pattern
        private static async Task<PendingReservationRequest> CreateReservationRequest(
            VisitingTime? visitingTime = null)
        {
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (10, 00),
                FinishTime = (20, 00),
                TablesWithNumberOfSeats = new byte[] {6, 8, 10}
            }.BuildAsync();


            visitingTime ??= VisitingTime.TryCreate(16, 00).Value!;

            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(4).Value!;

            var visitorId = new VisitorId(Guid.NewGuid());

            var result = restaurant.TryCreateReservationRequest(
                numberOfRequestedSeats,
                visitingTime,
                visitorId,
                SystemTimeStub.Instance);

            result.ThrowIfNotSuccessful();

            return result.Value!;
        }
    }
}