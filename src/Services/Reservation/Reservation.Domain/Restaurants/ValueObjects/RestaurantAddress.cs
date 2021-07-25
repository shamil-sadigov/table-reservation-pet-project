#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.ValueObjects
{
    public sealed class RestaurantAddress : SingleValueObject<string>
    {
        private RestaurantAddress(string value) : base(value)
        {
        }

        public static Result<RestaurantAddress> TryCreate(string address)
        {
            if (address.IsNullOrEmpty())
                return new Error("address should contain value");

            return new RestaurantAddress(address);
        }
    }
}