using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Restaurants.Domain.ReservationRequests
{
    public class ReservationId : GuidIdentity
    {
        public ReservationId(Guid value) : base(value)
        {
        }
    }
}