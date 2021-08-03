using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

namespace Reservations.Domain.ReservationRequests.DomainRules
{
    public class VisitingTimeMustNotPassRule : IDomainRule
    {
        private readonly DateTime _visitingDateTime;

        public VisitingTimeMustNotPassRule(
            DateTime visitingDateTime)
        {
            _visitingDateTime = visitingDateTime;
        }
        
        public Result Check()
        {
            if (SystemClock.DateTimeNow > _visitingDateTime)
            {
                return new Error($"Visiting time {_visitingDateTime} has been already " +
                                 $"passed current time {SystemClock.DateTimeNow}");
            }

            return Result.Success();
        }
    }
}