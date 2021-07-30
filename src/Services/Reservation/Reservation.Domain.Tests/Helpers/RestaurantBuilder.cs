#region

using System;
using System.Threading.Tasks;
using Moq;
using MoreLinq;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;

#endregion

namespace Reservation.Domain.Tests.Helpers
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
                
                var tableUniquenessCheckerMock = CreateTableUniquenessChecker(restaurant, tableId);

                var addToTableResult = await restaurant.TryAddTableAsync(
                    tableId,
                    numberOfSeats,
                    tableUniquenessCheckerMock.Object);
                
                addToTableResult.ThrowIfNotSuccessful();
            });

            restaurant.ClearDomainEvents();
            
            return restaurant;
        }

        private static Mock<ITableUniquenessChecker> CreateTableUniquenessChecker(Restaurant restaurant, TableId tableId)
        {
            var tableUniquenessCheckerMock = new Mock<ITableUniquenessChecker>();

            tableUniquenessCheckerMock.Setup(x => x.IsUniqueAsync(restaurant.Id, tableId))
                .ReturnsAsync(true);
            
            return tableUniquenessCheckerMock;
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