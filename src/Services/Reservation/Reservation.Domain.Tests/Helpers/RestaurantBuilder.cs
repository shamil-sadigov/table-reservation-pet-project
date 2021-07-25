#region

using System;
using System.Threading.Tasks;
using Moq;
using MoreLinq;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Domain.Tests.Helpers
{
    public record RestaurantBuilder
    {
        public string Name { get; init; }

        public (byte hours, byte minutes) StartTime { get; init; }

        public (byte hours, byte minutes) FinishTime { get; init; }

        public string Address { get; init; }

        public byte[] TablesWithNumberOfSeats { get; init; }

        public async Task<Restaurant> BuildAsync(bool clearDomainEvent = true)
        {
            var workingHours = CreateWorkingHours();
            var address = CreateAddress();

            var checker = new Mock<IRestaurantUniquenessChecker>();

            checker.Setup(x => x.IsUniqueAsync(Name, address.Value))
                .ReturnsAsync(true);
            
            var result = await Restaurant.TryCreateAsync(
                Name,
                workingHours,
                address,
                checker.Object);

            result.ThrowIfNotSuccessful();

            Restaurant restaurant = result.Value!;

            TablesWithNumberOfSeats?.ForEach(number =>
            {
                var creationResult = NumberOfSeats.TryCreate(number);
                creationResult.ThrowIfNotSuccessful();

                NumberOfSeats numberOfSeats = creationResult.Value!;

                var addToTableResult = restaurant.TryAddTable(numberOfSeats);
                addToTableResult.ThrowIfNotSuccessful();
            });

            restaurant.ClearDomainEvents();
            
            return restaurant;
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