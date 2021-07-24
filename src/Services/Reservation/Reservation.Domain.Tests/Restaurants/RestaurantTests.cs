#region

using System;
using FluentAssertions;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tests.Helpers;
using Xunit;

#endregion

namespace Reservation.Domain.Tests.Restaurants
{
    public class RestaurantTests
    {
        [Fact]
        public void Can_create_restaurant()
        {
            // Arrange
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;
            var address = RestaurantAddress.TryCreate("Some address").Value!;

            // Act
            var result = Restaurant.TryCreate(
                "name",
                workingHours,
                address);

            // Assert
            result.ShouldSucceed();

            var registeredRestaurant = result.Value!;

            NewRestaurantCreatedDomainEvent publishedDomainEvent =
                registeredRestaurant.ShouldHavePublishedDomainEvent<NewRestaurantCreatedDomainEvent>();

            publishedDomainEvent.RestaurantId
                .Should()
                .Be(registeredRestaurant.Id);

            publishedDomainEvent.RestaurantWorkingHours
                .Should()
                .Be(workingHours);

            publishedDomainEvent.RestaurantAddress
                .Should()
                .Be(address);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        public void Can_add_table_to_restaurant(byte number)
        {
            // Arrange 
            var restaurant = new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00)
            }.Build();

            var numberOfSeats = NumberOfSeats.TryCreate(number).Value!;

            // Act
            var addToTableResult = restaurant.TryAddTable(numberOfSeats);

            // Assert
            addToTableResult.ShouldSucceed();

            var publishedDomainEvent =
                restaurant.ShouldHavePublishedDomainEvent<NewTableAddedToRestaurantDomainEvent>();

            publishedDomainEvent.RestaurantId
                .Should()
                .Be(restaurant.Id);

            publishedDomainEvent.NumberOfSeats
                .Value
                .Should()
                .Be(number);

            publishedDomainEvent.TableId
                .Should()
                .NotBeNull();
        }
        
        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        public void Can_create_reservation_request_when_restaurant_has_available_table(byte numberOfSeats)
        {
            // Arrange
            var restaurant = new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00),
                TablesWithNumberOfSeats = new byte[] {2, 4, 2, 6, 8}
            }.Build();

            var numberOfRequestedSeats = NumberOfSeats.TryCreate(numberOfSeats).Value!;
            VisitingTime visitingTime = VisitingTime.TryCreate(hours: 12, minutes: 00).Value!;

            // Act
            var result = restaurant.TryCreateReservationRequest(numberOfRequestedSeats, visitingTime);

            // Assert
            result.ShouldSucceed();

            ReservationRequest reservationRequest = result.Value!;

            ReservationIsRequestedDomainEvent publishedDomainEvent
                = reservationRequest.ShouldHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();

            publishedDomainEvent.RequestedTableId
                .Should()
                .NotBeNull();

            publishedDomainEvent.ReservationRequestId
                .Should()
                .NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void Cannot_create_reservation_request_when_number_of_requested_seats_is_too_small(byte numberOfSeats)
        {
            // Arrange
            var restaurant = new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00),
                TablesWithNumberOfSeats = new byte[] {8, 10, 12, 14}
            }.Build();

            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(numberOfSeats).Value!;
            VisitingTime visitingTime = VisitingTime.TryCreate(hours: 12, minutes: 00).Value!;

            // Act
            var result = restaurant.TryCreateReservationRequest(numberOfRequestedSeats, visitingTime);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Table * is too big for requested number of seats *");
            restaurant.ShouldNotHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(20, 10)]
        [InlineData(22, 00)]
        public void Cannot_create_ReservationRequest_when_restaurant_is_not_open(byte hours, byte minutes)
        {
            // Arrange
            var restaurant = new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (12, 00),
                FinishTime = (20, 00),
                TablesWithNumberOfSeats = new byte[] {6, 8, 10}
            }.Build();

            VisitingTime visitTime = VisitingTime.TryCreate(hours, minutes).Value!;
            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(4).Value!;

            // Act
            var result = restaurant.TryCreateReservationRequest(numberOfRequestedSeats, visitTime);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike($"Restaurant {restaurant.Id} is not open at {visitTime} time");
            restaurant.ShouldNotHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();
        }
    }
}