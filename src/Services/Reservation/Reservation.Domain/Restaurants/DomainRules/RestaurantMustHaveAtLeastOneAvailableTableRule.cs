#region

using System.Collections.Generic;
using System.Linq;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.DomainRules
{
    internal sealed class RestaurantMustHaveAtLeastOneAvailableTableRule : IDomainRule
    {
        private readonly NumberOfSeats _numberOfSeats;
        private readonly List<Table> _tables;

        public RestaurantMustHaveAtLeastOneAvailableTableRule(List<Table> tables, NumberOfSeats numberOfSeats)
        {
            _tables = tables;
            _numberOfSeats = numberOfSeats;
        }

        public Result Check()
        {
            var availableTableExists = _tables
                .Where(table => table.IsAvailable)
                .Any(x => x.HasAtLeast(_numberOfSeats));

            return availableTableExists
                ? Result.Success()
                : new Error($"No available table found with {_numberOfSeats} number of seats");
        }
    }
}