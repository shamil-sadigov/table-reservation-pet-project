#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservations.Domain.ReservationRequestRejections.DomainRules
{
    public class RejectionTimeMustNotPassCurrentSystemTimeRule : IDomainRule
    {
        private readonly DateTime _rejectionDateTime;

        public RejectionTimeMustNotPassCurrentSystemTimeRule(DateTime rejectionDateTime)
        {
            _rejectionDateTime = rejectionDateTime;
        }

        public Result Check()
        {
            if (_rejectionDateTime > SystemClock.DateTimeNow)
                return new Error($"Rejection date '{_rejectionDateTime}' " +
                                 $"must not pass current system time '{SystemClock.DateTimeNow}'");

            return Result.Success();
        }
    }
}