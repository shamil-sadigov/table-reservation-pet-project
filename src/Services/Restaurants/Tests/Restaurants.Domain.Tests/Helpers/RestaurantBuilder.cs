#region

using System.Collections.Generic;
using System.Linq;
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
    public class RestaurantBuilder
    {
        private  RestaurantName _name;
        private RestaurantAddress _address;
        private RestaurantWorkingHours _workingHours;
        private List<(TableId tableId, NumberOfSeats numberOfSeats)> _tableInfos;

        public RestaurantBuilder(RestaurantName name)
        {
            _name = name;
        }
        
        public static RestaurantBuilder WithName(string name)
        {
            var nameResult =  RestaurantName.TryCreate(name);
            nameResult.ThrowIfNotSuccessful();
            return new RestaurantBuilder(nameResult.Value!);
        }

        public RestaurantBuilder LocatingAt(string address)
        {
            var addressResult = RestaurantAddress.TryCreate(address);

            addressResult.ThrowIfNotSuccessful();

            _address =  addressResult.Value!;

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

                NumberOfSeats numberOfSeats = numberOfSeatsResult.Value!;
                
                TableId tableId = tableIdResult.Value!;

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

            _workingHours =  result.Value!;
            return this;
        }
        
        public async Task<Restaurant> BuildAsync(bool clearDomainEvent = true)
        {
            var checker = new Mock<IRestaurantUniquenessChecker>();

            checker.Setup(x => x.IsUniqueAsync(_name, _address))
                .ReturnsAsync(true);

            var result = await Restaurant.TryCreateAsync(
                _name,
                _workingHours,
                _address,
                checker.Object);

            result.ThrowIfNotSuccessful();

            Restaurant restaurant = result.Value!;
            
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