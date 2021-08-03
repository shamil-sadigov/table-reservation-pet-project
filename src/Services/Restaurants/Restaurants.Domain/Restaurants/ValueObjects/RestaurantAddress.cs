#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using BuildingBlocks.Helpers;

#endregion

namespace Restaurants.Domain.Restaurants.ValueObjects
{
    public sealed class RestaurantAddress : SingleValueObject<string>
    {
        private RestaurantAddress(string value) : base(value)
        {
        }

        public static Result<RestaurantAddress> TryCreate(string address)
        {
            if (address.IsNullOrWhiteSpace() || address.Length<3)
                return new Error("address should contain value and has at least 3 characters");

            return new RestaurantAddress(address);
        }
    }
}