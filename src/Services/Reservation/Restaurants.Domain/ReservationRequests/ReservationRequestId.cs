#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Restaurants.Domain.ReservationRequests
{
    public class ReservationRequestId : GuidIdentity
    {
        public ReservationRequestId(Guid id)
            : base(id)
        {
        }
    }
}