#region

using System;
using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Tables;
using Reservation.Domain.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Reservation.Domain.Tests.Restaurants
{
    public class RestaurantTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RestaurantTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Can_register_restaurant()
        {
            // Arrange
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;
            var address = RestaurantAddress.TryCreate("Some address").Value!;

            // Act
            var result = Restaurant.TryRegisterNew(
                "name",
                workingHours,
                address);

            // Assert
            result.ShouldSucceed();

            var registeredRestaurant = result.Value!;

            NewRestaurantRegisteredDomainEvent publishedDomainEvent =
                registeredRestaurant.ShouldHavePublishedDomainEvent<NewRestaurantRegisteredDomainEvent>();

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
            var restaurant = CreateRestaurant();
            var numberOfSeats = NumberOfSeats.TryCreate(number).Value!;

            // Act
            var result = restaurant.TryAddTable(numberOfSeats);

            // Assert
            result.ShouldSucceed();

            var publishedDomainEvent =
                restaurant.ShouldHavePublishedDomainEvent<NewTableAddedToRestaurantDomainEvent>();

            publishedDomainEvent.RestaurantId
                .Should()
                .Be(restaurant.Id);

            publishedDomainEvent.NumberOfSeats
                .Should()
                .Be(numberOfSeats);

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
            var restaurant = CreateRestaurant();
            AddTablesToRestaurant(restaurant, numbersOfSeatsPerTable: new byte[] {2, 4, 2, 6, 8});
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
        public void Cannot_create_reservation_request_when_number_of_requested_seats_is_too_big(byte numberOfSeats)
        {
            // Arrange
            var restaurant = CreateRestaurant();
            AddTablesToRestaurant(restaurant, numbersOfSeatsPerTable: new byte[] {6, 8, 10});
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
        public void Cannot_create_ReservationRequest_at_time_when_restaurant_is_not_open(byte hours, byte minutes)
        {
            // Arrange
            var startWorkingTime = new TimeSpan(12, 00, 00);
            var finishWorkingTime = new TimeSpan(20, 00, 00);

            var restaurant = CreateRestaurant(startWorkingTime, finishWorkingTime);
            AddTablesToRestaurant(restaurant, numbersOfSeatsPerTable: new byte[] {6, 8, 10});

            VisitingTime visitTime = VisitingTime.TryCreate(hours, minutes).Value!;

            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(4).Value!;

            // Act
            var result = restaurant.TryCreateReservationRequest(numberOfRequestedSeats, visitTime);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike($"Restaurant {restaurant.Id} is not open at {visitTime} time");
            restaurant.ShouldNotHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();
        }


        private static Restaurant CreateRestaurant(
            TimeSpan? startWorkingTime = null,
            TimeSpan? finishWorkingTime = null)
        {
            startWorkingTime ??= new TimeSpan(10, 00, 00);
            finishWorkingTime ??= new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(
                startWorkingTime.Value,
                finishWorkingTime.Value).Value!;

            var address = RestaurantAddress.TryCreate("Some address").Value!;

            var result = Restaurant.TryRegisterNew(
                "name",
                workingHours,
                address);

            EnsureResultSuccessful(result);

            var restaurant = result.Value!;

            restaurant.ClearDomainEvents();

            return restaurant;
        }


        private static void AddTablesToRestaurant(
            Restaurant restaurant,
            params byte[] numbersOfSeatsPerTable)
        {
            foreach (var numberOfSeats in numbersOfSeatsPerTable)
            {
                var tableSizeForTwo = NumberOfSeats.TryCreate(numberOfSeats).Value!;

                var result = restaurant.TryAddTable(tableSizeForTwo);

                EnsureResultSuccessful(result);

                restaurant.ClearDomainEvents();
            }
        }

        private static void EnsureResultSuccessful(Result result)
        {
            if (result.Failed) throw new InvalidOperationException("Failed to add table to a restaurant");
        }
    }
}