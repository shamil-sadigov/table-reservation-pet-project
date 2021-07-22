#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.DomainRules;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class Restaurant : Entity, IAggregateRoot
    {
        private readonly List<Table> _tables;
        private RestaurantAddress _address;
        private RestaurantWorkingHours _restaurantWorkingHours;

        private Restaurant(string name, RestaurantWorkingHours restaurantWorkingHours, RestaurantAddress address)
        {
            Id = new RestaurantId(Guid.NewGuid());
            _restaurantWorkingHours = restaurantWorkingHours;
            _address = address;
            _tables = new List<Table>();

            AddDomainEvent(new NewRestaurantRegisteredDomainEvent(
                Id, 
                name, 
                restaurantWorkingHours, 
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

            AddDomainEvent(new NewTableAddedInRestaurantDomainEvent(
                Id, 
                newTable.Id, 
                numberOfSeats));
            
            return Result.Success();
        }

        public Result<ReservationRequest> CreateReservationRequest(NumberOfSeats numberOfSeats)
        {
            var rule = new RestaurantMustBeOpen(_restaurantWorkingHours)
                .And(new RestaurantMustHaveAtLeastOneAvailableTable(_tables, numberOfSeats));

            var result = rule.Check();
            
            if (result.Failed)
            {
                return result.WithResponse<ReservationRequest>(null);
            }



            throw new NotImplementedException();



        }
    }
}