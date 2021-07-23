#region

using System;
using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.DomainRules;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class Restaurant : Entity, IAggregateRoot
    {
        private List<Table> _tables;
        private RestaurantAddress _address;
        private string _name;
        private RestaurantWorkingHours _workingHours;

        // for EF
        private Restaurant()
        {
            
        }
        
        private Restaurant(string name, RestaurantWorkingHours workingHours, RestaurantAddress address)
        {
            Id = new RestaurantId(Guid.NewGuid());
            _name = name;
            _workingHours = workingHours;
            _address = address;
            _tables = new List<Table>();

            AddDomainEvent(new NewRestaurantRegisteredDomainEvent(
                Id, 
                name, 
                workingHours, 
                address));
        }

        public RestaurantId Id { get; }

        public static Result<Restaurant> TryRegisterNew(
            string name,
            RestaurantWorkingHours restaurantWorkingHours,
            RestaurantAddress address)
        {
            if (ContainsNullValues(new {name, workingHours = restaurantWorkingHours, address}, out var errors)) return errors;

            return new Restaurant(name, restaurantWorkingHours, address);
        }


        public Result TryAddTable(NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {numberOfSeats}, out var errors)) 
                return errors;

            var result = Table.TryCreate(Id, numberOfSeats);
            
            if (result.Failed)
                return result;

            Table newTable = result.Value!;

            _tables.Add(newTable);
            
            return Result.Success();
        }

        public Result<ReservationRequest> TryCreateReservationRequest(
            NumberOfSeats numberOfSeats,
            VisitingTime visitingTime)
        {
            var rule = new RestaurantMustBeOpenAtVisitingTime(Id, visitingTime, _workingHours)
                .And(new RestaurantMustHaveAtLeastOneAvailableTable(_tables, numberOfSeats));

            var result = rule.Check();
            
            if (result.Failed)
                return result.WithResponse<ReservationRequest>(null);

            var availableTable = FindAvailableTableWithMinimumNumberOfSeats(numberOfSeats);
            
             result = availableTable.CanBeReserved(numberOfSeats);

             if (result.Failed)
                 return result.WithResponse<ReservationRequest>(null);

             return ReservationRequest.TryCreate(availableTable.Id, visitingTime);
        }

        /// <returns>
        ///     Returns Table that matches <paramref name="numberOfSeats"/>
        ///     AND number of seats is minimum.
        ///     Example:
        ///         We have two available tables with 6 and 10 number of seats.
        ///         if <paramref name="numberOfSeats"/> is 4 number of seats
        ///         then we should return table with 6 number of seats
        ///         as it is the minimum we can provide.
        /// </returns>
        private Table FindAvailableTableWithMinimumNumberOfSeats(NumberOfSeats numberOfSeats)
        {
            return _tables
                .Where(table => table.IsAvailable)
                .Where(x => x.HasAtLeast(numberOfSeats))
                .OrderBy(x=> x.NumberOfSeats)
                .First();

            return null;
        }
    }
}