using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Reservation.Domain.ReservationRequests
{
    public class ReservationId : GuidIdentity
    {
        public ReservationId(Guid value) : base(value)
        {
        }
    }
}