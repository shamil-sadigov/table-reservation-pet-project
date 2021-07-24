#region

using System;
using BuildingBlocks.Domain;

#endregion

namespace Reservation.Domain.Restaurants.ValueObjects
{
    public class ReservationRequestId : GuidIdentity
    {
        public ReservationRequestId(Guid id)
            : base(id)
        {
        }
    }
}