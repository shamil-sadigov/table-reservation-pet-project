using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Reservation.Domain.ReservationRequests
{
    public class ReservationRequestRejectionId : GuidIdentity
    {
        public ReservationRequestRejectionId(Guid value) : base(value)
        {
        }
    }
}