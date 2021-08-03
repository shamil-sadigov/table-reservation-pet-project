#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Reservations.Domain.Reservations.DomainRules
{
    public class ApprovalTimeMustNotPassCurrentSystemTimeRule : IDomainRule
    {
        private readonly DateTime _approvalDateTime;

        public ApprovalTimeMustNotPassCurrentSystemTimeRule(DateTime approvalDateTime)
        {
            _approvalDateTime = approvalDateTime;
        }

        public Result Check()
        {
            if (_approvalDateTime > SystemClock.DateTimeNow)
                return new Error($"Approval date '{_approvalDateTime}' " +
                                 $"must not pass current system time '{SystemClock.DateTimeNow}'");

            return Result.Success();
        }
    }
}