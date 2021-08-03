#region

using System;
using System.Threading.Tasks;
using BuildingBlocks.Tests.Shared;
using Moq;
using MoreLinq;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurants.Domain.Tests.Helpers
{
    public record RestaurantBuilder
    {
        public string Name { get; init; }

        public (byte hours, byte minutes) StartTime { get; init; }

        public (byte hours, byte minutes) FinishTime { get; init; }

        public string Address { get; init; }

        public (string tableId, byte numberOfSeats)[] TablesInfo { get; init; }

        public async Task<Restaurant> BuildAsync(bool clearDomainEvent = true)
        {
            var workingHours = CreateWorkingHours();
            var address = CreateAddress();

            var checker = new Mock<IRestaurantUniquenessChecker>();

            checker.Setup(x => x.IsUniqueAsync(Name, address))
                .ReturnsAsync(true);

            var result = await Restaurant.TryCreateAsync(
                Name,
                workingHours,
                address,
                checker.Object);

            result.ThrowIfNotSuccessful();

            Restaurant restaurant = result.Value!;

            TablesInfo?.ForEach(async tableInfo =>
            {
                (TableId tableId, NumberOfSeats numberOfSeats) = CreateTableIdAndNumberOfSeats(tableInfo);

                var addToTableResult = restaurant.TryAddTable(
                    tableId,
                    numberOfSeats);

                addToTableResult.ThrowIfNotSuccessful();
            });

            restaurant.ClearAllDomainEvents();

            return restaurant;
        }

        private static (TableId tableId, NumberOfSeats numberOfSeats)
            CreateTableIdAndNumberOfSeats((string tableId, byte numberOfSeats) table)
        {
            var numberOfSeatsResult = NumberOfSeats.TryCreate(table.numberOfSeats);
            var tableIdResult = TableId.TryCreate(table.tableId);

            numberOfSeatsResult
                .CombineWith(tableIdResult)
                .ThrowIfNotSuccessful();

            NumberOfSeats numberOfSeats = numberOfSeatsResult.Value!;
            TableId tableId = tableIdResult.Value!;

            return (tableId, numberOfSeats);
        }

        private RestaurantAddress CreateAddress()
        {
            var result = RestaurantAddress.TryCreate(Address);

            result.ThrowIfNotSuccessful();

            return result.Value!;
        }

        private RestaurantWorkingHours CreateWorkingHours()
        {
            var startWorkingTime = new TimeSpan(StartTime.hours, StartTime.minutes, 00);
            var finishWorkingTime = new TimeSpan(FinishTime.hours, FinishTime.minutes, 00);

            var result = RestaurantWorkingHours.TryCreate(
                startWorkingTime,
                finishWorkingTime);

            result.ThrowIfNotSuccessful();

            return result.Value!;
        }
    }
}