#region

using System;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.Tests;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.DomainEvents;
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
        [Theory]
        [InlineData(15, 00, 13, 00)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(19, 15, 15, 20)]
        public async Task Can_approve_reservation_request(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            var reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvalDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            var systemTimeMock = new Mock<ISystemTime>();

            var timeOfTheDay = new TimeSpan(approvalHours, approvalMinutes, 0) + 1.Minutes();

            systemTimeMock.Setup(x => x.DateTimeNow)
                .Returns(() => DateTime.Today + timeOfTheDay);

            // Act
            var result = reservationRequest.TryApprove(
                administratorId,
                approvalDateTime,
                systemTimeMock.Object);

            // Assert
            result.ShouldSucceed();

            Domain.ReservationRequests.Reservation reservation = result.Value!;

            var domainEvent = reservation.ShouldHavePublishedDomainEvent<ReservationIsMade>();

            domainEvent.ApprovedByAdministratorId
                .Should()
                .Be(administratorId);

            domainEvent.ReservationRequestId
                .Should()
                .Be(reservationRequest.Id);

            domainEvent.ApprovalDateTime
                .Should()
                .Be(approvalDateTime);
        }

        [Theory]
        [InlineData(15, 00, 15, 15)]
        [InlineData(10, 15, 11, 15)]
        [InlineData(13, 15, 13, 20)]
        public async Task Cannot_approve_reservation_request_when_visiting_dateTime_has_expired(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            ReservationRequest reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvedDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            // Act
            var result = reservationRequest.TryApprove(
                administratorId,
                approvedDateTime,
                SystemTimeStub.Instance);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike(
                "Cannot approve reservation request * visitingDateTime is expired");
        }

        [Theory]
        [InlineData(15, 00, 14, 15)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(17, 15, 12, 20)]
        public async Task Cannot_approve_reservation_when_approval_dateTime_is_greater_than_currentSystemTimeDateTime(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            ReservationRequest reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvalDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            var systemTimeMock = new Mock<ISystemTime>();
            var timeOfTheDay = new TimeSpan(approvalHours, approvalMinutes, 0) - 1.Minutes();

            systemTimeMock.Setup(x => x.DateTimeNow)
                .Returns(() => DateTime.Today + timeOfTheDay);

            // Act
            var result = reservationRequest.TryApprove(
                administratorId,
                approvalDateTime,
                systemTimeMock.Object);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Approval date * must not be greater than current system date *");
        }

        [Theory]
        [InlineData(15, 00, 13, 00)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(19, 15, 15, 20)]
        public async Task Can_reject_reservationRequest(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            var reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var rejectionDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            var systemTimeMock = new Mock<ISystemTime>();

            var timeOfTheDay = new TimeSpan(approvalHours, approvalMinutes, 0) + 1.Minutes();

            systemTimeMock.Setup(x => x.DateTimeNow)
                .Returns(() => DateTime.Today + timeOfTheDay);

            // Act
            var result = reservationRequest.TryReject(
                administratorId,
                rejectionDateTime,
                systemTimeMock.Object,
                "rejection reason");

            // Assert
            result.ShouldSucceed();

            ReservationRequestRejection reservationRequestRejection = result.Value!;

            var domainEvent =
                reservationRequestRejection.ShouldHavePublishedDomainEvent<ReservationRequestIsRejected>();

            domainEvent.RejectedByAdministratorId
                .Should()
                .Be(administratorId);

            domainEvent.RejectionReason
                .Should()
                .Be("rejection reason");

            domainEvent.RejectedDateTime
                .Should()
                .Be(rejectionDateTime);

            domainEvent.ReservationRequestId
                .Should()
                .Be(reservationRequest.Id);
        }

        [Theory]
        [InlineData(15, 00, 15, 15)]
        [InlineData(10, 15, 11, 15)]
        [InlineData(13, 15, 13, 20)]
        public async Task Cannot_reject_reservationRequest_when_visiting_dateTime_has_expired(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            ReservationRequest reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvedDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            // Act
            var result = reservationRequest.TryReject(
                administratorId,
                approvedDateTime,
                SystemTimeStub.Instance,
                "rejection reason");

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike(
                "Cannot reject pending reservation request * visitingDateTime is expired");
        }


        [Theory]
        [InlineData(15, 00, 14, 15)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(17, 15, 12, 20)]
        public async Task
            Cannot_reject_reservationRequest_when_rejectionDateTime_is_greater_than_currentSystemTimeDateTime(
                byte visitingHours,
                byte visitingMinutes,
                byte approvalHours,
                byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            ReservationRequest reservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvalDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            var systemTimeMock = new Mock<ISystemTime>();
            var timeOfTheDay = new TimeSpan(approvalHours, approvalMinutes, 0) - 1.Minutes();

            systemTimeMock.Setup(x => x.DateTimeNow)
                .Returns(() => DateTime.Today + timeOfTheDay);

            // Act
            var result = reservationRequest.TryReject(
                administratorId,
                approvalDateTime,
                systemTimeMock.Object,
                "rejection reason");

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike(
                "Rejection date * must not be greater than current system date *");
        }

        // TODO: Extract to builder pattern
        private static async Task<ReservationRequest> RequestReservation(
            VisitingTime? visitingTime = null)
        {
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (10, 00),
                FinishTime = (20, 00),
                TablesInfo = new (string tableId, byte numberOfSeats)[]
                {
                    ("TBL-1", 6), ("TBL-2", 8), ("TBL-3", 10)
                }
            }.BuildAsync();


            visitingTime ??= VisitingTime.TryCreate(16, 00).Value!;

            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(4).Value!;

            var visitorId = new VisitorId(Guid.NewGuid());

            var result = restaurant.TryRequestReservation(
                numberOfRequestedSeats,
                visitingTime,
                visitorId,
                SystemTimeStub.Instance);

            result.ThrowIfNotSuccessful();

            return result.Value!;
        }
    }
}