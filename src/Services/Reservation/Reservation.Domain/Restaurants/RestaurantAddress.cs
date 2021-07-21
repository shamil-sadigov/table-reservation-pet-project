#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class RestaurantAddress : ValueObject
    {
        private string _address;

        private RestaurantAddress(string address)
        {
            _address = address;
        }

        public static Result<RestaurantAddress> TryCreate(string address)
        {
            if (address.IsNullOrEmpty())
                return new Error( "address should contain value");

            return new RestaurantAddress(address);
        }
    }
}