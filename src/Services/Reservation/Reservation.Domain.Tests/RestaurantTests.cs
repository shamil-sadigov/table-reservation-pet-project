using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.BusinessRule;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Tables;
using Xunit;

namespace Reservation.Domain.Tests
{
    public class RestaurantTests
    {
        // TODO: Add more tests
        
        [Fact]
        public void Can_create_restaurant()
        {
            var startWorkingTime = new TimeSpan(10, 00, 00);
            var finishWorkingTime = new TimeSpan(23, 00, 00);

            var workingHours = WorkingHours.TryCreate(startWorkingTime, finishWorkingTime).Value;

            var addressResult = RestaurantAddress.TryCreate("Some address").Value;

            var tableSize2 = TableSize.TryCreate(2).Value;
            var tableSize4 = TableSize.TryCreate(4).Value;
            var tableSize8 = TableSize.TryCreate(8).Value;

            var tablesInfo = new List<NewTableInfo>()
            {
                new(tableSize2),
                new(tableSize4),
                new(tableSize2),
                new(tableSize4),
                new(tableSize8),
                new(tableSize8),
            };
            
            Result<Restaurant> result = Restaurant.TryRegisterNew(
                "name", 
                workingHours,
                addressResult,
                tablesInfo);
            
            result.ShouldSucceed();
        }
    }
}