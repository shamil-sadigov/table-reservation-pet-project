#region

using System;
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
    }
}