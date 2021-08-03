#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservations.Domain.Reservations
{
    public class ReservationId : GuidIdentity
    {
        public ReservationId(Guid value) : base(value)
        {
        }
    }
}