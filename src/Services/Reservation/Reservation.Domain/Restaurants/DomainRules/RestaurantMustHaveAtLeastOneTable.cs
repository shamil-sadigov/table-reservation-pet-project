#region

using System.Collections.Generic;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using MoreLinq;

#endregion

namespace Reservation.Domain.Restaurants.DomainRules
{
    public sealed class RestaurantMustHaveAtLeastOneTable : IDomainRule
    {
        private readonly IReadOnlyCollection<NewTableInfo> _tables;

        public RestaurantMustHaveAtLeastOneTable(IReadOnlyCollection<NewTableInfo> tables)
        {
            _tables = tables;
        }

        public Result Check()
        {
            return !_tables.AtLeast(1)
                ? Result.Failure("Restaurant must contain at least one table")
                : Result.Success();
        }
    }
}