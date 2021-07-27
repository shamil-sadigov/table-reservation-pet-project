using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Reservation.Domain.ReservationRequests
{
    public sealed class AdministratorId : GuidIdentity
    {
        public AdministratorId(Guid id) : base(id)
        {
        }
    }
}