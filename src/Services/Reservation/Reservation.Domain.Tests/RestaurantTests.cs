#region

using System;
using BuildingBlocks.Domain.DomainRules;
using FluentAssertions;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Tables;
using Xunit;
using Xunit.Abstractions;

#endregion

// TODO: Test published domain events
namespace Reservation.Domain.Tests
{
    public class RestaurantTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RestaurantTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
        // TODO: Add more tests

        [Fact]
        public void Can_create_restaurant()
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
        }

        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(8)]
        public void Can_add_table_in_restaurant(byte number)
        {
            // Arrange
            var restaurant = CreateRestaurant();
            var numberOfSeats = NumberOfSeats.TryCreate(number).Value!;

            // Act
            var result = restaurant.TryAddTable(numberOfSeats);

            // Assert
            result.ShouldSucceed();
        }


        [Theory]
        [InlineData(2)]
        [InlineData(4)]
        [InlineData(8)]
        public void Can_get_available_table_if_restaurant_has_one_with_specified_number_of_seats(byte number)
        {
            // Arrange
            var restaurant = CreateRestaurant();
            AddTablesToRestaurant(restaurant, numbersOfSeatsPerTable: new byte[]{ 2, 4, 2, 6, 8});
            var numberOfSeats = NumberOfSeats.TryCreate(number).Value!;

            // Act
            restaurant.CreateReservationRequest(numberOfSeats);

            // Assert
            // availableTable.Should().NotBeNull();
        }
        
        
        [Theory]
        [InlineData(6)]
        [InlineData(8)]
        [InlineData(10)]
        public void Cannot_get_available_table_if_restaurant_has_no_table_with_specified_number_of_seats(byte numberOfSeats)
        {
            // Arrange
            var restaurant = CreateRestaurant();
            AddTablesToRestaurant(restaurant, numbersOfSeatsPerTable: new byte[]{ 2, 3, 3, 4, 4});
            var tableSize = NumberOfSeats.TryCreate(numberOfSeats).Value!;

            // Act
            // Table? availableTable = restaurant.CreateReservationRequest(tableSize);
            //
            // // Assert
            // availableTable.Should().BeNull();
        }

        


        private static Restaurant CreateRestaurant()
        {
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = RestaurantWorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value!;

            var address = RestaurantAddress.TryCreate("Some address").Value!;

            var result = Restaurant.TryRegisterNew(
                "name",
                workingHours,
                address);

            EnsureResultSuccessful(result);

            return result.Value!;
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
            }
        }
        
        private static void EnsureResultSuccessful(Result result)
        {
            if (result.Failed) throw new InvalidOperationException("Failed to add table to a restaurant");
        }
    }
}