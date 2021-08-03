#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.DomainEvents;
using Restaurants.Domain.Restaurants.DomainRules;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables;
using Restaurants.Domain.Tables.ValueObjects;
using Restaurants.Domain.Visitors.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants
{
    public sealed class Restaurant : Entity, IAggregateRoot
    {
        private readonly List<Table> _tables;
        private readonly RestaurantWorkingHours _workingHours;
        private RestaurantAddress _address;
        private RestaurantName _name;

        // for EF
        private Restaurant()
        {
        }

        private Restaurant(RestaurantName name, RestaurantWorkingHours workingHours, RestaurantAddress address)
        {
            Id = new RestaurantId(Guid.NewGuid());
            _name = name;
            _workingHours = workingHours;
            _address = address;
            _tables = new List<Table>();

            AddDomainEvent(new RestaurantCreatedDomainEvent(
                Id,
                name,
                workingHours,
                address));
        }

        public RestaurantId Id { get; }

        public static async Task<Result<Restaurant>> TryCreateAsync(
            RestaurantName name,
            RestaurantWorkingHours restaurantWorkingHours,
            RestaurantAddress address,
            IRestaurantUniquenessChecker uniquenessChecker)
        {
            if (ContainsNullValues(new {name, restaurantWorkingHours, address, uniquenessChecker}, out var errors))
                return errors;

            var rule = new RestaurantMustBeUniqueRule(uniquenessChecker, name, address);

            var result = await rule.CheckAsync();

            return result.Failed
                ? result.WithoutValue<Restaurant>()
                : new Restaurant(name, restaurantWorkingHours, address);
        }

        public Result TryAddTable(
            TableId tableId,
            NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {tableId, numberOfSeats}, out var errors))
                return errors;

            var rule = new TableInRestaurantMustBeUniqueRule(_tables, Id, tableId);

            var ruleCheckResult = rule.Check();

            if (ruleCheckResult.Failed)
                return ruleCheckResult;

            var tableResult = Table.TryCreate(tableId, Id, numberOfSeats);

            if (tableResult.Failed)
                return tableResult;

            Table newTable = tableResult.Value!;

            _tables.Add(newTable);

            return Result.Success();
        }

        public Result TryRequestReservation(
            NumberOfSeats numberOfSeats,
            VisitingTime visitingTime,
            VisitorId visitorId)
        {
            var rule = new RestaurantMustBeOpenAtVisitingTimeRule(Id, visitingTime, _workingHours)
                .And(new RestaurantMustHaveAtLeastOneAvailableTableRule(_tables, numberOfSeats));

            var result = rule.Check();

            if (result.Failed)
                return result;

            var availableTable = FindAvailableTableWithMinimumNumberOfSeats(numberOfSeats);

            if (!availableTable.CanBeReserved(numberOfSeats))
                return result;

            var visitingDateTIme = SystemClock.DateNow + visitingTime.AsTimeSpan();

            AddDomainEvent(new TableReservationIsRequestedDomainEvent(
                Id,
                availableTable.Id,
                visitingDateTIme,
                visitorId));

            return Result.Success();
        }

        /// <returns>
        ///     Returns Table that matches <paramref name="numberOfSeats" />
        ///     AND number of seats is minimum.
        ///     Example:
        ///     We have two available tables with 6 and 10 number of seats.
        ///     if <paramref name="numberOfSeats" /> is 4 number of seats
        ///     then we should return table with 6 number of seats
        ///     as it is the minimum we can provide.
        /// </returns>
        private Table FindAvailableTableWithMinimumNumberOfSeats(NumberOfSeats numberOfSeats)
        {
            return _tables
                .Where(table => table.IsAvailable)
                .Where(x => x.HasAtLeast(numberOfSeats))
                .OrderBy(x => x.NumberOfSeats)
                .First();
        }
    }
}