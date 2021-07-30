#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservation.Domain.ReservationRequests.DomainRules
{
    public class ApprovalDateTimeMustNotBeFutureDateRule : IDomainRule
    {
        private readonly DateTime _approvalDateTime;
        private readonly ISystemTime _systemTime;

        public ApprovalDateTimeMustNotBeFutureDateRule(DateTime approvalDateTime, ISystemTime systemTime)
        {
            _approvalDateTime = approvalDateTime;
            _systemTime = systemTime;
        }

        public Result Check()
        {
            if (_approvalDateTime > _systemTime.DateTimeNow)
                return new Error($"Approval date '{_approvalDateTime}' " +
                                 $"must not be greater than current system date '{_systemTime.DateTimeNow}'");

            return Result.Success();
        }
    }
}