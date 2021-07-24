#region

using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.ValueObjects
{
    public sealed class RestaurantAddress : ValueObject
    {
        public string Value { get; }

        private RestaurantAddress(string value)
        {
            Value = value;
        }

        public static Result<RestaurantAddress> TryCreate(string address)
        {
            if (address.IsNullOrEmpty())
                return new Error("address should contain value");

            return new RestaurantAddress(address);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}