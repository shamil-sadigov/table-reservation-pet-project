#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Tests.Shared;
using Moq;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurant.Tests.Shared
{
    public class RestaurantBuilder
    {
        private RestaurantAddress _address;
        private readonly RestaurantName _name;
        private List<(TableId tableId, NumberOfSeats numberOfSeats)> _tableInfos;
        private RestaurantWorkingHours _workingHours;

        public RestaurantBuilder(RestaurantName name)
        {
            _name = name;
        }

        public static RestaurantBuilder WithName(string name)
        {
            var nameResult = RestaurantName.TryCreate(name);
            nameResult.ThrowIfNotSuccessful();
            return new RestaurantBuilder(nameResult.Value!);
        }

        public RestaurantBuilder LocatingAt(string address)
        {
            var addressResult = RestaurantAddress.TryCreate(address);

            addressResult.ThrowIfNotSuccessful();

            _address = addressResult.Value!;

            return this;
        }


        public RestaurantBuilder WithTables(params (string tableId, byte numberOfSeats)[] tableInfos)
        {
            _tableInfos = tableInfos.Select(table =>
            {
                var numberOfSeatsResult = NumberOfSeats.TryCreate(table.numberOfSeats);
                var tableIdResult = TableId.TryCreate(table.tableId);

                numberOfSeatsResult
                    .CombineWith(tableIdResult)
                    .ThrowIfNotSuccessful();

                var numberOfSeats = numberOfSeatsResult.Value!;

                var tableId = tableIdResult.Value!;

                return (tableId, numberOfSeats);
            }).ToList();

            return this;
        }

        public RestaurantBuilder WithWorkingSchedule(string startTime, string finishTime)
        {
            var startWorkingTime = startTime.AsTimeSpan();
            var finishWorkingTime = finishTime.AsTimeSpan();

            var result = RestaurantWorkingHours.TryCreate(
                startWorkingTime,
                finishWorkingTime);

            result.ThrowIfNotSuccessful();

            _workingHours = result.Value!;
            return this;
        }

        public async Task<Restaurants.Domain.Restaurants.Restaurant> BuildAsync(bool clearDomainEvent = true)
        {
            var checker = new Mock<IRestaurantChecker>();

            checker.Setup(x => x.ExistsAsync(_name, _address))
                .ReturnsAsync(false);

            var result = await Restaurants.Domain.Restaurants.Restaurant.TryCreateAsync(
                _name,
                _workingHours,
                _address,
                checker.Object);

            result.ThrowIfNotSuccessful();

            var restaurant = result.Value!;

            _tableInfos?.ForEach(async tableInfo =>
            {
                var addToTableResult = restaurant.TryAddTable(
                    tableInfo.tableId,
                    tableInfo.numberOfSeats);

                addToTableResult.ThrowIfNotSuccessful();
            });

            restaurant.ClearAllDomainEvents();

            return restaurant;
        }
    }
}