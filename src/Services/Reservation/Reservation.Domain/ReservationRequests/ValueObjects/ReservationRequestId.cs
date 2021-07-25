#region

using System;
using BuildingBlocks.Domain;

#endregion

namespace Reservation.Domain.ReservationRequests.ValueObjects
{
    public class ReservationRequestId : GuidIdentity
    {
        public ReservationRequestId(Guid id)
            : base(id)
        {
        }
    }
}