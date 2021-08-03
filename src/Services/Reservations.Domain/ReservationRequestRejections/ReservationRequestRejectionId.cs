#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequestRejections
{
    public class ReservationRequestRejectionId : GuidIdentity
    {
        public ReservationRequestRejectionId(Guid value) : base(value)
        {
        }
    }
}