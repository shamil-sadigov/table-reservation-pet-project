#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Restaurants;
using Reservation.Domain.Restaurants.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables.DomainRules;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class Table : Entity
    {
        private RestaurantId _restaurantId;
        private readonly TableStatus _status;

        // for EF
        private Table()
        {
        }

        private Table(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            NumberOfSeats = numberOfSeats;
            _restaurantId = restaurantId;
            Id = new TableId(Guid.NewGuid());
            _status = TableStatus.Available;

            AddDomainEvent(new NewTableAddedToRestaurantDomainEvent(
                restaurantId,
                Id,
                numberOfSeats));
        }

        public TableId Id { get; }
        internal NumberOfSeats NumberOfSeats { get; }

        internal bool IsAvailable => _status == TableStatus.Available;

        internal static Result<Table> TryCreate(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {restaurantId, numberOfSeats}, out var errors))
                return errors;

            return new Table(restaurantId, numberOfSeats);
        }

        internal bool HasAtLeast(NumberOfSeats numberOfSeats) => NumberOfSeats >= numberOfSeats;
        
        internal Result CanBeReserved(NumberOfSeats requestedNumberOfSeats)
        {
            var rule = new OnlyAvailableTableCanBeReserved(Id, _status)
                .And(new RequestedNumberOfSeatsMustNotBeTooSmall(Id, NumberOfSeats, requestedNumberOfSeats));

            var result = rule.Check();

            return result;
        }
    }
}