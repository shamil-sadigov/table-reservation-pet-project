#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.Restaurants;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class Table : Entity
    {
        internal RestaurantId RestaurantId { get; }
        internal  NumberOfSeats NumberOfSeats { get; }
        internal  TableId Id { get; }
        internal  TableStatus Status { get; }

        // for EF
        private Table()
        {
        }

        private Table(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            NumberOfSeats = numberOfSeats;
            RestaurantId = restaurantId;
            Id = new TableId(Guid.NewGuid());
            Status = TableStatus.Available;
        }


        public static Result<Table> TryCreate(RestaurantId restaurantId, NumberOfSeats numberOfSeats)
        {
            if (ContainsNullValues(new {restaurantId, tableSize = numberOfSeats}, out var errors))
            {
                return errors;
            }

            return new Table(restaurantId, numberOfSeats);
        }
        
        public bool IsAvailable => Status == TableStatus.Available;
    }
}