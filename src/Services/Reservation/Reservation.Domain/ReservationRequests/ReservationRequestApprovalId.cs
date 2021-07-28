using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Reservation.Domain.ReservationRequests
{
    public class ReservationRequestApprovalId : GuidIdentity
    {
        public ReservationRequestApprovalId(Guid value) : base(value)
        {
        }
    }
}