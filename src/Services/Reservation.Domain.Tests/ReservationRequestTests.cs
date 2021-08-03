#region

using System;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using Objectivity.AutoFixture.XUnit2.AutoMoq.Attributes;
using Objectivity.AutoFixture.XUnit2.Core.Attributes;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequestRejections;
using Reservations.Domain.ReservationRequestRejections.DomainEvents;
using Reservations.Domain.ReservationRequests;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Reservations.DomainEvents;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Reservation.Domain.Tests
{
    public class ReservationRequestTests
    {
        [Theory]
        [InlineAutoMockData("15:00", "13:00")]
        [InlineAutoMockData("10:20", "10:10")]
        [InlineAutoMockData("18:15", "15:20")]
        public void Can_approve_reservation_request(
            string visitingTime,
            string currentTime,
            RestaurantId restaurantId,
            VisitorId visitorId,
            AdministratorId administratorId,
            [CustomizeWith(typeof(LocalDatesCustomization))] TableId tableId)
        {
            // Arrange
            SetCurrentSystemTime(currentTime);

            var reservationRequest = ReservationRequest.TryCreate(
                    restaurantId, 
                    tableId, 
                    visitorId,
                    visitingDateTime: SystemClock.DateNow + visitingTime.AsTimeSpan())
                .Value!;
            
            // Act
            var result = reservationRequest.TryApprove(administratorId);
            
            // Assert
            result.ShouldSucceed();
            
            var reservation = result.Value!;
            
            var domainEvent = reservation.ShouldHavePublishedDomainEvent<ReservationIsCreatedDomainEvent>();
            
            domainEvent.ApprovedByAdministratorId
                .Should().Be(administratorId);
            
            domainEvent.ReservationRequestId
                .Should().Be(reservationRequest.Id);
            
            domainEvent.ReservationId
                .Should().Be(reservation.Id);
            
            domainEvent.TableId
                .Should().Be(tableId);
            
            domainEvent.RestaurantId
                .Should().Be(restaurantId);
        }
        
        [Theory]
        [InlineAutoMockData("15:00", "15:15")]
        [InlineAutoMockData("10:20", "11:15")]
        [InlineAutoMockData("18:15", "19:00")]
        public void Cannot_approve_reservation_request_when_visiting_dateTime_has_been_passed(
            string visitingTime,
            string currentTime,
            RestaurantId restaurantId,
            VisitorId visitorId,
            AdministratorId administratorId,
            [CustomizeWith(typeof(LocalDatesCustomization))] TableId tableId)
        {
            // Arrange
            SetCurrentSystemTime(currentTime);
            
            var reservationRequest = ReservationRequest.TryCreate(
                    restaurantId, 
                    tableId, 
                    visitorId,
                    visitingDateTime: DateTime.Today + visitingTime.AsTimeSpan())
                .Value!;
            
            // Act
            var result = reservationRequest.TryApprove(administratorId);
        
            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Visiting time * has been already passed *");
        }
        
        [Theory]
        [InlineAutoMockData("15:00", "13:00")]
        [InlineAutoMockData("10:20", "10:10")]
        [InlineAutoMockData("18:15", "15:20")]
        public void Can_reject_reservationRequest(string visitingTime, string currentTime, 
            RestaurantId restaurantId,
            VisitorId visitorId,
            AdministratorId administratorId,
            [CustomizeWith(typeof(LocalDatesCustomization))] TableId tableId,
            [CustomizeWith(typeof(LocalDatesCustomization))] RejectionReason rejectionReason)
        {
            // Arrange
            SetCurrentSystemTime(currentTime);
            
            var reservationRequest = ReservationRequest.TryCreate(
                    restaurantId, 
                    tableId, 
                    visitorId,
                    visitingDateTime: SystemClock.DateNow + visitingTime.AsTimeSpan())
                .Value!;
            
            // Act
            var result = reservationRequest.TryReject(administratorId, rejectionReason);
        
            // Assert
            result.ShouldSucceed();
        
            ReservationRequestRejection reservationRequestRejection = result.Value!;
        
            var domainEvent =
                reservationRequestRejection.ShouldHavePublishedDomainEvent<ReservationRequestIsRejected>();
        
            domainEvent.RejectedByAdministratorId
                .Should().Be(administratorId);
        
            domainEvent.RejectionReason
                .Should().Be(rejectionReason);
            
            domainEvent.ReservationRequestId
                .Should().Be(reservationRequest.Id);
            
            domainEvent.ReservationRequestRejectionId
                .Should().NotBeNull();
        }
        
        [Theory]
        [InlineAutoMockData("15:00", "15:15")]
        [InlineAutoMockData("10:20", "11:15")]
        [InlineAutoMockData("18:15", "19:00")]
        public void Cannot_reject_reservationRequest_when_visiting_dateTime_has_been_passed(
            string visitingTime, 
            string currentTime,
            RestaurantId restaurantId,
            VisitorId visitorId,
            AdministratorId administratorId,
            [CustomizeWith(typeof(LocalDatesCustomization))] TableId tableId,
            [CustomizeWith(typeof(LocalDatesCustomization))] RejectionReason rejectionReason)
        {
            // Arrange
            SetCurrentSystemTime(currentTime);
            
            var reservationRequest = ReservationRequest.TryCreate(
                    restaurantId, 
                    tableId, 
                    visitorId,
                    visitingDateTime: SystemClock.DateNow + visitingTime.AsTimeSpan())
                .Value!;
            
            // Act
            var result = reservationRequest.TryReject(administratorId,rejectionReason);
        
            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Visiting time * has been already passed *");
        }
        
        private static void SetCurrentSystemTime(string currentTime)
        {
            var currentSystemDateTime = DateTime.Today + currentTime.AsTimeSpan();
            SystemClock.Set(currentSystemDateTime);
        }
    }
}