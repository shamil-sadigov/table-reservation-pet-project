#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.DomainRules;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors;
using Reservation.Domain.Visitors.ValueObjects;

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

            AddDomainEvent(new RestaurantCreatedDomainEvent(
                Id, 
                name, 
                workingHours, 
                address));
        }

        public RestaurantId Id { get; }

        public static async Task<Result<Restaurant>> TryCreateAsync(
            string name,
            RestaurantWorkingHours restaurantWorkingHours,
            RestaurantAddress address,
            IRestaurantUniquenessChecker uniquenessChecker)
        {
            if (ContainsNullValues(new {name, restaurantWorkingHours, address}, out var errors))
                return errors;
            
            var rule = new RestaurantMustBeUniqueRule(uniquenessChecker, name, address);
            
            var result =  await rule.CheckAsync();

            if (result.Failed)
                return result.WithoutValue<Restaurant>();
            
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
            VisitingTime visitingTime,
            VisitorId visitorId)
        {
            var rule = new RestaurantMustBeOpenAtVisitingTimeRule(Id, visitingTime, _workingHours)
                .And(new RestaurantMustHaveAtLeastOneAvailableTableRule(_tables, numberOfSeats));
            
            var result = rule.Check();
            
            if (result.Failed)
                return result.WithoutValue<ReservationRequest>();

            var availableTable = FindAvailableTableWithMinimumNumberOfSeats(numberOfSeats);
            
             result = availableTable.CanBeReserved(numberOfSeats);
             
             if (result.Failed)
                 return result.WithoutValue<ReservationRequest>();
             
             return ReservationRequest.TryCreate(availableTable.Id,
                                      numberOfSeats,
                                      visitingTime,
                                      visitorId);
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
        }
    }
    
    
    
    
    
    
    
    
}