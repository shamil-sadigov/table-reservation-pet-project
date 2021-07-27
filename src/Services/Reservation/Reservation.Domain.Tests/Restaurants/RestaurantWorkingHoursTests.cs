#region

using System;
using FluentAssertions;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tests.Helpers;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Reservation.Domain.Tests.Restaurants
{
    public class RestaurantWorkingHoursTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RestaurantWorkingHoursTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData(10, 00, 55, 00, "finishTime should not be greater that 23:59:59")]
        [InlineData(03, 00, 22, 00, "startTime should be in range 06:00:00-23:59:59")]
        [InlineData(18, 00, 10, 00, "startTime should not be greater than finishTime")]
        public void Cannot_create_workingHours_when_startTime_or_finishTime_are_invalid(
            int startHour,
            int startMinutes,
            int finishHour,
            int finishMinutes,
            string expectedErrorMessage)
        {
            // Arrange
            var startTime = new TimeSpan(startHour, startMinutes, seconds: 00);
            var finishTime = new TimeSpan(finishHour, finishMinutes, seconds: 00);

            // Act
            var result = RestaurantWorkingHours.TryCreate(startTime, finishTime);

            // Assert
            result.ShouldFail();
            result.Errors!.ShouldContainSomethingLike(expectedErrorMessage);
        }


        [Theory]
        [InlineData(10, 00, 19, 00)]
        [InlineData(06, 00, 22, 30)]
        [InlineData(09, 00, 10, 00)]
        public void Can_create_workingHours_when_startTime_or_finishTime_are_valid(
            int startHour,
            int startMinutes,
            int finishHour,
            int finishMinutes)
        {
            // Arrange
            var startTime = new TimeSpan(startHour, startMinutes, 00);
            var finishTime = new TimeSpan(finishHour, finishMinutes, 00);

            // Act
            var result = RestaurantWorkingHours.TryCreate(startTime, finishTime);

            // Assert
            result.ShouldSucceed();
        }

        [Theory]
        [InlineData(09, 15)]
        [InlineData(11, 30)]
        [InlineData(13, 45)]
        [InlineData(15, 45)]
        [InlineData(18, 45)]
        public void Specified_time_is_restaurant_working_time(int hours, int minutes)
        {
            // Arrange
            var workingHours = CreateWorkingHours(fromHours: 09, toHours: 20);

            var flintstonesDateTime = new DateTime(
                year: 1,
                month: 1,
                day: 1,
                hours,
                minutes,
                second: 0);

            var timeOfDay = flintstonesDateTime.TimeOfDay;

            // Act & Assert
            workingHours.IsWorkingTime(timeOfDay)
                .Should()
                .BeTrue();
        }

        [Theory]
        [InlineData(06, 15)]
        [InlineData(08, 45)]
        [InlineData(20, 05)]
        [InlineData(23, 00)]
        [InlineData(00, 30)]
        public void Specified_time_is_not_working_time_of_a_restaurant(int hours, int minutes)
        {
            // Arrange
            var workingHours = CreateWorkingHours(fromHours: 09, toHours: 20);

            var flintstonesDateTime = new DateTime(
                year: 1,
                month: 1,
                day: 1,
                hours,
                minutes,
                second: 0);

            var timeOfDay = flintstonesDateTime.TimeOfDay;

            // Act & Assert
            workingHours.IsWorkingTime(timeOfDay)
                .Should()
                .BeFalse();
        }
        
        private static RestaurantWorkingHours CreateWorkingHours(int fromHours, int toHours)
        {
            var startTime = new TimeSpan(fromHours, 00, 00);
            var finishTime = new TimeSpan(toHours, 00, 00);

            var workingTime = RestaurantWorkingHours.TryCreate(startTime, finishTime).Value!;
            return workingTime;
        }
    }
}