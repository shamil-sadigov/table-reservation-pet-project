#region

using BuildingBlocks.Domain.BusinessRule;
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
                return Result<RestaurantAddress>.Failure("address should contain value");

            var restaurantAddress = new RestaurantAddress(address);

            return Result<RestaurantAddress>.Success(restaurantAddress);
        }
    }
}