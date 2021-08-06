#region

using System;
using System.Threading.Tasks;
using BuildingBlocks.Tests.Shared;
using FluentAssertions;
using Moq;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;
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
            var name = RestaurantName.TryCreate("restaurant-name").Value!;

            var uniquenessChecker = new Mock<IRestaurantUniquenessChecker>();

            uniquenessChecker.Setup(x => x.IsUniqueAsync(name, address))
                .ReturnsAsync(true);

            // Act
            var result = await Restaurant.TryCreateAsync(
                name,
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
                .Should().Be(name);
        }

        [Fact]
        public async Task Cannot_create_restaurant_when_name_and_address_is_not_unique()
        {
            // Arrange
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;
            var address = RestaurantAddress.TryCreate("Some address").Value!;
            var name = RestaurantName.TryCreate("restaurant-name").Value!;

            var checker = new Mock<IRestaurantUniquenessChecker>();

            checker.Setup(x => x.IsUniqueAsync(name, address))
                .ReturnsAsync(false);

            // Act
            var result = await Restaurant.TryCreateAsync(
                name,
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
            var restaurant = await RestaurantBuilder
                                        .WithName("restaurant name")
                                        .LocatingAt("restaurant address")
                                        .WithWorkingSchedule("09:00", "22:00")
                                        .BuildAsync();
            
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
            var restaurant = await RestaurantBuilder
                .WithName("restaurant name")
                .LocatingAt("restaurant address")
                .WithWorkingSchedule("09:00", "22:00")
                .WithTables(("TBL-1", numberOfSeats: 3))
                .BuildAsync();

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
        public async Task Can_create_reservation_request_when_restaurant_has_available_table(byte numberOfSeats_)
        {
            // Arrange
            var restaurant = await RestaurantBuilder
                .WithName("restaurant name")
                .LocatingAt("restaurant address")
                .WithWorkingSchedule("09:00", "22:00")
                .WithTables(
                    ("TBL-1", numberOfSeats: 2),
                    ("TBL-2", numberOfSeats: 4),
                    ("TBL-3", numberOfSeats: 2),
                    ("TBL-4", numberOfSeats: 8))
                .BuildAsync();

            var (numberOfSeats, visitingTime, visitorId) = CreateReservationParameters(numberOfSeats_);

            // Act
            var result = restaurant.TryRequestReservation(
                numberOfSeats,
                visitingTime,
                visitorId);

            // Assert
            result.ShouldSucceed();

            var publishedDomainEvent =
                restaurant.ShouldHavePublishedDomainEvent<TableReservationIsRequestedDomainEvent>();

            publishedDomainEvent.TableId
                .Should().NotBeNull();

            publishedDomainEvent.VisitorId
                .Should().Be(visitorId);

            publishedDomainEvent.VisitingDateTime.TimeOfDay
                .Should().Be(visitingTime.Value);
        }

       

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public async Task Cannot_create_reservation_request_when_number_of_requested_seats_is_too_small(
            byte numberOfSeats_)
        {
            // Arrange
            var restaurant = await RestaurantBuilder
                .WithName("restaurant name")
                .LocatingAt("restaurant address")
                .WithWorkingSchedule("09:00", "22:00")
                .WithTables(
                    ("TBL-1", numberOfSeats: 8),
                    ("TBL-2", numberOfSeats: 10),
                    ("TBL-3", numberOfSeats: 12),
                    ("TBL-4", numberOfSeats: 14))
                .BuildAsync();
            
            var (numberOfSeats, visitingTime, visitorId) = CreateReservationParameters(numberOfSeats_);
            
            // Act
            var result = restaurant.TryRequestReservation(
                numberOfSeats,
                visitingTime,
                visitorId);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike("Table * is too big for requested number of seats *");
            restaurant.ShouldNotHavePublishedDomainEvent<TableReservationIsRequestedDomainEvent>();
        }

        [Theory]
        [InlineData(10, 30)]
        [InlineData(20, 10)]
        [InlineData(22, 00)]
        public async Task Cannot_create_ReservationRequest_when_restaurant_is_not_open(byte hours, byte minutes)
        {
            // Arrange
            var restaurant = await RestaurantBuilder
                .WithName("restaurant name")
                .LocatingAt("restaurant address")
                .WithWorkingSchedule("12:00", "20:00")
                .WithTables(
                    ("TBL-1", numberOfSeats: 6),
                    ("TBL-2", numberOfSeats: 8),
                    ("TBL-3", numberOfSeats: 10),
                    ("TBL-4", numberOfSeats: 6))
                .BuildAsync();
            
            var (numberOfSeats, visitingTime, visitorId) = 
                CreateReservationParameters(visitingTimeHours: hours, visitingTimeMinutes: minutes);
            
            // Act
            var result = restaurant.TryRequestReservation(
                numberOfSeats,
                visitingTime,
                visitorId);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike($"Restaurant {restaurant.Id} is not open at {visitingTime} time");
            restaurant.ShouldNotHavePublishedDomainEvent<TableReservationIsRequestedDomainEvent>();
        }
        
        private static (NumberOfSeats numberOfSeats, VisitingTime visitingTime, VisitorId visitorId) 
            CreateReservationParameters(
                byte numberOfSeats = 4,
                byte visitingTimeHours = 12,
                byte visitingTimeMinutes = 00)
        {
            var numberOfRequestedSeats = NumberOfSeats.TryCreate(numberOfSeats).Value!;
            var visitingTime = new VisitingTime(new TimeSpan(visitingTimeHours, visitingTimeMinutes, 0));
            var visitorId = new VisitorId(Guid.NewGuid());
            
            return (numberOfRequestedSeats, visitingTime, visitorId);
        }
    }
}