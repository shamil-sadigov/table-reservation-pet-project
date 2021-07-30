using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.AsyncVersion;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

namespace Reservation.Domain.Restaurants.DomainRules
{
    public class TableInRestaurantMustBeUniqueRule:IDomainRuleAsync
    {
        private readonly ITableUniquenessChecker _checker;
        private readonly RestaurantId _restaurantId;
        private readonly TableId _tableId;

        public TableInRestaurantMustBeUniqueRule(
            ITableUniquenessChecker checker,
            RestaurantId restaurantId,
            TableId tableId)
        {
            _checker = checker;
            _restaurantId = restaurantId;
            _tableId = tableId;
        }
        
        public async Task<Result> CheckAsync()
        {
            var isTableUnique = await _checker.IsUniqueAsync(_restaurantId, _tableId);

            return isTableUnique 
                ? Result.Success() 
                : new Error($"Restaurant '{_restaurantId}' already has table '{_tableId}'");
        }
    }
}