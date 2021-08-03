#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequests.ValueObjects
{
    public sealed class RestaurantId : GuidIdentity
    {
        public RestaurantId(Guid id) : base(id)
        {
        }
    }
}