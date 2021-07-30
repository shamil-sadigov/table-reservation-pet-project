#region

using System;
using BuildingBlocks.Domain.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public sealed class AdministratorId : GuidIdentity
    {
        public AdministratorId(Guid id) : base(id)
        {
        }
    }
}