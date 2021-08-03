#region

using System;
using System.Threading.Tasks;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using Moq;
using Restaurants.Domain.ReservationRequests;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.DomainEvents;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Tables.DomainEvents;
using Restaurants.Domain.Tables.ValueObjects;
using Restaurants.Domain.Tests.Helpers;
using Restaurants.Domain.Visitors.ValueObjects;
using Xunit;

#endregion

namespace Restaurants.Domain.Tests.Restaurants
{
    public class RestaurantTests
    {
        [Fact]
        public async Task Can_create_restaurant()
        {
            // Arrange
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;
            var address = RestaurantAddress.TryCreate("Some address").Value!;

            var uniquenessChecker = new Mock<IRestaurantUniquenessChecker>();

            uniquenessChecker.Setup(x => x.IsUniqueAsync("restaurant-name", address))
                .ReturnsAsync(true);

            // Act
            var result = await Restaurant.TryCreateAsync(
                "restaurant-name",
                workingHours,
                address,
                uniquenessChecker.Object);

            // Assert
            result.ShouldSucceed();

            var registeredRestaurant = result.Value!;

            var publishedDomainEvent = registeredRestaurant
                .ShouldHavePublishedDomainEvent<RestaurantCreatedDomainEvent>();

            publishedDomainEvent.RestaurantId
                .Should().Be(registeredRestaurant.Id);

            publishedDomainEvent.RestaurantWorkingHours
                .Should().Be(workingHours);

            publishedDomainEvent.RestaurantAddress
                .Should().Be(address);

            publishedDomainEvent.Name
                .Should().Be("restaurant-name");
        }

        [Fact]
        public async Task Cannot_create_restaurant_when_name_and_address_is_not_unique()
        {
            // Arrange
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;
            var address = RestaurantAddress.TryCreate("Some address").Value!;

            var checker = new Mock<IRestaurantUniquenessChecker>();

            checker.Setup(x => x.IsUniqueAsync("restaurant-name", address))
                .ReturnsAsync(false);

            // Act
            var result = await Restaurant.TryCreateAsync(
                "restaurant-name",
                workingHours,
                address,
                checker.Object);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Restaurant * already exists");
        }


        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        public async Task Can_add_table_to_restaurant(byte numberOfSeats_)
        {
            // Arrange 
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00)
            }.BuildAsync();

            var numberOfSeats = NumberOfSeats.TryCreate(numberOfSeats_).Value!;
            var tableId = TableId.TryCreate("TBL-1").Value!;

            // Act
            var result = restaurant.TryAddTable(
                tableId,
                numberOfSeats);

            // Assert
            result.ShouldSucceed();

            var publishedDomainEvent =
                restaurant.ShouldHavePublishedDomainEvent<TableAddedToRestaurantDomainEvent>();

            publishedDomainEvent.RestaurantId
                .Should().Be(restaurant.Id);

            publishedDomainEvent.NumberOfSeats
                .Should().Be(numberOfSeats);

            publishedDomainEvent.TableId
                .Should().Be(tableId);
        }


        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        public async Task Cannot_add_table_to_restaurant_when_the_same_table_already_exists(byte numberOfSeats_)
        {
            // Arrange 
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00),
                TablesInfo = new (string tableId, byte numberOfSeats)[]
                {
                    ("TBL-1", 3)
                }
            }.BuildAsync();

            var numberOfSeats = NumberOfSeats.TryCreate(numberOfSeats_).Value!;
            var tableId = TableId.TryCreate("TBL-1").Value!;

            // Act
            var result = restaurant.TryAddTable(
                tableId,
                numberOfSeats);

            // Assert
            result.ShouldFail();
            result.Errors.ShouldContainSomethingLike($"Restaurant * already has table '{tableId}'");
            restaurant.ShouldNotHavePublishedDomainEvent<TableAddedToRestaurantDomainEvent>();
        }


        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(6)]
        public async Task Can_create_reservation_request_when_restaurant_has_available_table(byte numberOfSeats)
        {
            // Arrange
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00),
                TablesInfo = new (string tableId, byte numberOfSeats)[]
                {
                    ("TBL-1", 2), ("TBL-2", 4), ("TBL-3", 2), ("TBL-4", 8)
                }
            }.BuildAsync();

            var numberOfRequestedSeats = NumberOfSeats.TryCreate(numberOfSeats).Value!;
            VisitingTime visitingTime = VisitingTime.TryCreate(hours: 12, minutes: 00).Value!;
            var visitorId = new VisitorId(Guid.NewGuid());

            // Act
            var result = restaurant.TryRequestReservation(
                numberOfRequestedSeats,
                visitingTime,
                visitorId);

            // Assert
            result.ShouldSucceed();
            
            var publishedDomainEvent = 
                restaurant.ShouldHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();

            publishedDomainEvent.RequestedTableId
                .Should().NotBeNull();

            publishedDomainEvent.VisitorId
                .Should().Be(visitorId);
            
            publishedDomainEvent.VisitingDateTime.TimeOfDay
                .Should().Be(visitingTime.AsTimeSpan());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Cannot_create_reservation_request_when_number_of_requested_seats_is_too_small(
            byte numberOfSeats)
        {
            // Arrange
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (09, 00),
                FinishTime = (22, 00),
                TablesInfo = new (string tableId, byte numberOfSeats)[]
                {
                    ("TBL-1", 8), ("TBL-2", 10), ("TBL-3", 12), ("TBL-4", 14)
                }
            }.BuildAsync();

            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(numberOfSeats).Value!;
            VisitingTime visitingTime = VisitingTime.TryCreate(hours: 12, minutes: 00).Value!;
            var visitorId = new VisitorId(Guid.NewGuid());

            // Act
            var result = restaurant.TryRequestReservation(
                numberOfRequestedSeats,
                visitingTime,
                visitorId);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Table * is too big for requested number of seats *");
            restaurant.ShouldNotHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(20, 10)]
        [InlineData(22, 00)]
        public async Task Cannot_create_ReservationRequest_when_restaurant_is_not_open(byte hours, byte minutes)
        {
            // Arrange
            var restaurant = await new RestaurantBuilder
            {
                Name = "restaurant name",
                Address = "restaurant address",
                StartTime = (12, 00),
                FinishTime = (20, 00),
                TablesInfo = new (string tableId, byte numberOfSeats)[]
                {
                    ("TBL-1", 6), ("TBL-2", 8), ("TBL-3", 10)
                }
            }.BuildAsync();

            VisitingTime visitTime = VisitingTime.TryCreate(hours, minutes).Value!;
            NumberOfSeats numberOfRequestedSeats = NumberOfSeats.TryCreate(4).Value!;
            var visitorId = new VisitorId(Guid.NewGuid());

            // Act
            var result = restaurant.TryRequestReservation(
                numberOfRequestedSeats,
                visitTime,
                visitorId);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike($"Restaurant {restaurant.Id} is not open at {visitTime} time");
            restaurant.ShouldNotHavePublishedDomainEvent<ReservationIsRequestedDomainEvent>();
        }
    }
}