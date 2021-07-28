#region

using System;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using MoreLinq;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Tests.Helpers;
using Reservation.Domain.Visitors.ValueObjects;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Reservation.Domain.Tests.ReservationRequests
{
    public class ReservationRequestTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public ReservationRequestTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(15, 00, 13, 00)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(19, 15, 15, 20)]
        public async Task Can_approve_pending_reservation_request(
            byte visitingHours,
            byte visitingMinutes,
            byte approvalHours,
            byte approvalMinutes)
        {
            // Arrange
            var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
            ReservationRequest? pendingReservationRequest = await RequestReservation(visitingTime);
            var administratorId = new AdministratorId(Guid.NewGuid());
            var approvalDateTime = DateTime.Today.Add(new TimeSpan(approvalHours, approvalMinutes, 0));

            var systemTimeMock = new Mock<ISystemTime>();

            var timeOfTheDay = new TimeSpan(approvalHours, approvalMinutes, 0) + 1.Minutes();
            
            systemTimeMock.Setup(x => x.DateTimeNow)
                .Returns(() => DateTime.Today + timeOfTheDay); 
            
            // Act
            var result = pendingReservationRequest.TryApprove(
                administratorId,
                approvalDateTime,
                systemTimeMock.Object);

            // Assert
            result.ShouldSucceed();
            
            var approvedReservationRequest = result.Value!;

            var domainEvent = approvedReservationRequest.ShouldHavePublishedDomainEvent<ReservationRequestIsApproved>();

            domainEvent.ApprovedByAdministratorId
                .Should()
                .Be(administratorId);

            domainEvent.ReservationRequestId
                .Should()
                .Be(pendingReservationRequest.Id);

            domainEvent.ApprovalDateTime
                .Should()
                .Be(approvalDateTime);
        }


        [Theory]
        [InlineData(15, 00, 15, 15)]
        [InlineData(10, 15, 11, 15)]
        [InlineData(13, 15, 13, 20)]
        public async Task Cannot_approve_pending_reservation_request_when_visiting_dateTime_has_expired(
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
                "Cannot approve pending reservation request * visitingDateTime is expired");
        }
        
        
        [Theory]
        [InlineData(15, 00, 14, 15)]
        [InlineData(10, 15, 10, 05)]
        [InlineData(17, 15, 12, 20)]
        public async Task Cannot_approve_pending_reservation_request_when_approved_dateTime_is_greater_than_currentSystemTime(
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

        
        
        //
        // [Theory]
        // [InlineData(15, 30, 15, 15)]
        // [InlineData(12, 40, 10, 55)]
        // [InlineData(19, 15, 13, 20)]
        // public async Task Can_reject_pending_reservation_request(
        //     byte visitingHours,
        //     byte visitingMinutes,
        //     byte rejectedHours,
        //     byte rejectedMinutes)
        // {
        //     // Arrange
        //     var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
        //     ReservationRequest pendingReservationRequest = await RequestReservation(visitingTime);
        //     var administratorId = new AdministratorId(Guid.NewGuid());
        //     var rejectedDate = DateTime.Today.Add(new TimeSpan(rejectedHours, rejectedMinutes, 0));
        //
        //     // Act
        //     Result<ReservationRequest.RejectedReservationRequest> result =
        //         pendingReservationRequest.TryReject(
        //             administratorId,
        //             rejectedDate,
        //             "rejection reason");
        //
        //     // Assert
        //     result.ShouldSucceed();
        //
        //     var rejectedRequest = result.Value!;
        //
        //     var domainEvent = rejectedRequest.ShouldHavePublishedDomainEvent<ReservationRequestIsRejected>();
        //
        //     domainEvent.RejectedByAdministratorId
        //         .Should()
        //         .Be(administratorId);
        //
        //     domainEvent.RejectedDateTime
        //         .Should()
        //         .Be(rejectedDate);
        //
        //     domainEvent.ReservationRequestId
        //         .Should()
        //         .Be(pendingReservationRequest.Id);
        //
        //     domainEvent.RejectionReason
        //         .Should()
        //         .Be("rejection reason");
        // }
        //
        // [Theory]
        // [InlineData(15, 00, 15, 15)]
        // [InlineData(10, 15, 11, 15)]
        // [InlineData(13, 15, 13, 20)]
        // public async Task Cannot_reject_pending_reservation_request_when_visiting_dateTime_has_expired(
        //     byte visitingHours,
        //     byte visitingMinutes,
        //     byte rejectedHours,
        //     byte rejectedMinutes)
        // {
        //     // Arrange
        //     var visitingTime = VisitingTime.TryCreate(visitingHours, visitingMinutes).Value;
        //     PendingReservationRequest pendingReservationRequest = await RequestReservation(visitingTime);
        //     var administratorId = new AdministratorId(Guid.NewGuid());
        //     var rejectedDate = DateTime.Today.Add(new TimeSpan(rejectedHours, rejectedMinutes, 0));
        //
        //     // Act
        //     Result<ReservationRequest.RejectedReservationRequest> result =
        //         pendingReservationRequest.TryReject(
        //             administratorId,
        //             rejectedDate,
        //             "rejection reason");
        //
        //     // Assert
        //     result.ShouldFail();
        //     result.Errors!.ShouldContainSomethingLike(
        //         "Cannot reject pending reservation request * visitingDateTime is expired");
        // }

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
                TablesWithNumberOfSeats = new byte[] {6, 8, 10}
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