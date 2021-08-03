#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequests.ValueObjects
{
    public class ReservationRequestId : GuidIdentity
    {
        public ReservationRequestId(Guid id)
            : base(id)
        {
        }
    }
}