#region

using System;
using System.Collections.Generic;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants
{
    public sealed class RestaurantId : GuidIdentity
    {
        public RestaurantId(Guid id):base(id)
        {
        }
    }
}