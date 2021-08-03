#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

#endregion

namespace Restaurants.Domain.ReservationRequests
{
    public class ApprovalTimeMustNotPassVisitingTimeRule : IDomainRule
    {
        private readonly DateTime _approvalDateTime;
        private readonly DateTime _visitingDateTime;

        public ApprovalTimeMustNotPassVisitingTimeRule(
            DateTime visitingDateTime,
            DateTime approvalDateTime)
        {
            _visitingDateTime = visitingDateTime;
            _approvalDateTime = approvalDateTime;
        }
        
        public Result Check()
        {
            if (_approvalDateTime > _visitingDateTime)
            {
                return new Error($"Approval dateTime '{_approvalDateTime}' must not pass" +
                                 $" visiting dateTime {_visitingDateTime}");
            }

            return Result.Success();
        }
    }
}