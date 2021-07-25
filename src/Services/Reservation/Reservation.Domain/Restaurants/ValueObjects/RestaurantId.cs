#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.ValueObjects
{
    public sealed class RestaurantId : GuidIdentity
    {
        public RestaurantId(Guid id) : base(id)
        {
        }
    }
}