#region

using System.Threading.Tasks;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.AsyncVersion;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.DomainRules
{
    public class RestaurantMustBeUniqueRule : IDomainRuleAsync
    {
        private readonly IRestaurantUniquenessChecker _checker;
        private readonly RestaurantAddress _restaurantAddress;
        private readonly string _restaurantName;

        public RestaurantMustBeUniqueRule(IRestaurantUniquenessChecker checker, string restaurantName,
            RestaurantAddress restaurantAddress)
        {
            _checker = checker;
            _restaurantName = restaurantName;
            _restaurantAddress = restaurantAddress;
        }

        public async Task<Result> CheckAsync()
        {
            var isRestaurantUnique = await _checker.IsUniqueAsync(_restaurantName, _restaurantAddress);

            return isRestaurantUnique
                ? Result.Success()
                : new Error(
                    $"Restaurant with name '{_restaurantName}' and address '{_restaurantAddress}' already exists");
        }
    }
}