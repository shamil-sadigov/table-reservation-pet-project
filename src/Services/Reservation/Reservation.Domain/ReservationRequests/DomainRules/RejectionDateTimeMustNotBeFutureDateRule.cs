using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;

namespace Reservation.Domain.ReservationRequests.DomainRules
{
    public class RejectionDateTimeMustNotBeFutureDateRule:IDomainRule
    {
        private readonly DateTime _rejectionDateTime;
        private readonly ISystemTime _systemTime;

        public RejectionDateTimeMustNotBeFutureDateRule(DateTime rejectionDateTime, ISystemTime systemTime)
        {
            _rejectionDateTime = rejectionDateTime;
            _systemTime = systemTime;
        }
        
        public Result Check()
        {
            if (_rejectionDateTime > _systemTime.DateTimeNow)
            {
                return new Error($"Rejection date '{_rejectionDateTime}' " +
                                 $"must not be greater than current system date '{_systemTime.DateTimeNow}'");
            }
            
            return Result.Success();
        }
    }
}