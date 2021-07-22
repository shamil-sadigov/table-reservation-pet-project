#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class RestaurantId : ValueObject
    {
        private readonly Guid _id;

        public RestaurantId(Guid id)
        {
            _id = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return _id;
        }
    }
}