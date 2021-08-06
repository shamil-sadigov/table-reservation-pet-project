#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables.DomainEvents;
using Restaurants.Domain.Tables.DomainRules;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurants.Domain.Tables
{
    public sealed class Table : Entity
    {
        private readonly TableState _state;
        private readonly RestaurantId _restaurantId;

        // for EF
        private Table()
        {
        }

        private Table(TableId tableId, RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            NumberOfSeats = numberOfSeats;
            _restaurantId = restaurantId;
            Id = tableId;
            _state = TableState.Available;

            AddDomainEvent(new TableAddedToRestaurantDomainEvent(
                restaurantId,
                Id,
                numberOfSeats));
        }

        public TableId Id { get; }
        
        internal NumberOfSeats NumberOfSeats { get; }

        internal bool IsAvailable => _state == TableState.Available;

        internal static Result<Table> TryCreate(TableId tableId, RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {restaurantId, numberOfSeats}, out var errors))
                return errors;

            return new Table(tableId, restaurantId, numberOfSeats);
        }

        internal bool HasAtLeast(NumberOfSeats numberOfSeats) => NumberOfSeats >= numberOfSeats;

        internal bool CanBeReserved(NumberOfSeats requestedNumberOfSeats)
        {
            var rule = new OnlyAvailableTableCanBeReservedRule(Id, _state)
                .And(new RequestedNumberOfSeatsMustNotBeTooSmallRule(Id, NumberOfSeats, requestedNumberOfSeats));

            var result = rule.Check();

            return result.Succeeded;
        }
    }
}