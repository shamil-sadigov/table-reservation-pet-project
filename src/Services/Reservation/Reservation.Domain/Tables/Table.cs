#region

using System;
using BuildingBlocks.Domain;
using Reservation.Domain.Restaurants;

#endregion

namespace Reservation.Domain.Tables
{
    public sealed class Table : Entity
    {
        private readonly RestaurantId _restaurantId;
        private readonly TableSize _tableSize;
        private TableId _id;
        private TableStatus _status;

        // for EF
        private Table()
        {
        }

        public Table(RestaurantId restaurantId, TableSize tableSize)
        {
            _id = new TableId(Guid.NewGuid());
            _restaurantId = restaurantId;
            _tableSize = tableSize;
            _status = TableStatus.Available;
        }
    }
}