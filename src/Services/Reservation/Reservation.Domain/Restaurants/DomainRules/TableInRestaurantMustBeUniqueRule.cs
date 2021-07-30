#region

using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

#endregion

namespace Reservation.Domain.Restaurants.DomainRules
{
    public class TableInRestaurantMustBeUniqueRule : IDomainRule
    {
        private readonly List<Table> _availableTables;
        private readonly RestaurantId _restaurantId;
        private readonly TableId _tableId;

        public TableInRestaurantMustBeUniqueRule(
            List<Table> availableTables,
            RestaurantId restaurantId,
            TableId tableId)
        {
            _availableTables = availableTables;
            _restaurantId = restaurantId;
            _tableId = tableId;
        }

        public Result Check()
        {
            var tableIsNotUnique = _availableTables.Any(x => x.Id == _tableId);

            return tableIsNotUnique
                ? new Error($"Restaurant '{_restaurantId}' already has table '{_tableId}'")
                : Result.Success();
        }
    }
}