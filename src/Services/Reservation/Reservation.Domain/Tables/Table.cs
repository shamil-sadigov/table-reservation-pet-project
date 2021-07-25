#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables.DomainRules;
using Reservation.Domain.Tables.ValueObjects;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class Table : Entity
    {
        private RestaurantId _restaurantId;
        private readonly TableState _state;

        // for EF
        private Table()
        {
        }

        private Table(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            NumberOfSeats = numberOfSeats;
            _restaurantId = restaurantId;
            Id = new TableId(Guid.NewGuid());
            _state = TableState.Available;

            AddDomainEvent(new TableAddedToRestaurantDomainEvent(
                restaurantId,
                Id,
                numberOfSeats));
        }

        public TableId Id { get; }
        internal NumberOfSeats NumberOfSeats { get; }

        internal bool IsAvailable => _state == TableState.Available;

        internal static Result<Table> TryCreate(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {restaurantId, numberOfSeats}, out var errors))
                return errors;

            return new Table(restaurantId, numberOfSeats);
        }

        internal bool HasAtLeast(NumberOfSeats numberOfSeats) => NumberOfSeats >= numberOfSeats;
        
        internal Result CanBeReserved(NumberOfSeats requestedNumberOfSeats)
        {
            var rule = new OnlyAvailableTableCanBeReservedRule(Id, _state)
                .And(new RequestedNumberOfSeatsMustNotBeTooSmallRule(Id, NumberOfSeats, requestedNumberOfSeats));

            var result = rule.Check();

            return result;
        }
    }
}