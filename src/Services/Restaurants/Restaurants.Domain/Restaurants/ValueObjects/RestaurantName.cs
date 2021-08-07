#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using BuildingBlocks.Helpers;

#endregion

namespace Restaurants.Domain.Restaurants.ValueObjects
{
    public sealed class RestaurantName : SingleValueObject<string>
    {
        private RestaurantName(string value) : base(value)
        {
        }

        public static Result<RestaurantName> TryCreate(string name)
        {
            if (name.IsNullOrWhiteSpace() || name.Length < 3)
                return new Error("name should not be null and have at least 3 characters");

            return new RestaurantName(name);
        }
    }
}