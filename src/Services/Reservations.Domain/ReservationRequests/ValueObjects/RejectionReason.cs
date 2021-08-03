#region

using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.ValueObjects;
using BuildingBlocks.Helpers;

#endregion

namespace Reservations.Domain.ReservationRequests.ValueObjects
{
    public sealed class RejectionReason : SingleValueObject<string>
    {
        private RejectionReason(string id) : base(id)
        {
        }

        public static Result<RejectionReason> TryCreate(string reason)
        {
            if (reason.IsNullOrWhiteSpace() || reason.Length < 5)
                return new Error("reason should not be null and have at least 5 characters");

            return new RejectionReason(reason);
        }
    }
}