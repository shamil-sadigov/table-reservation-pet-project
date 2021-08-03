using System;
using BuildingBlocks.Domain.ValueObjects;

namespace Reservations.Domain.ReservationRequests.ValueObjects
{
    public sealed class VisitorId : GuidIdentity
    {
        public VisitorId(Guid id) : base(id)
        {
        }
    }
}