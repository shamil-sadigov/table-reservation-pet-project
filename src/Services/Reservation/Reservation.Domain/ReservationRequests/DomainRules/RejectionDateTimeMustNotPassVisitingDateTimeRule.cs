using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.ReservationRequests.DomainRules
{
    public class RejectionDateTimeMustNotPassVisitingDateTimeRule:IDomainRule
    {
        private readonly ReservationRequestId _reservationRequestId;
        private readonly DateTime _visitingDateTime;
        private readonly DateTime _rejectedDateTime;

        public RejectionDateTimeMustNotPassVisitingDateTimeRule(
            ReservationRequestId reservationRequestId,
            DateTime visitingDateTime,
            DateTime rejectedDateTime)
        {
            _reservationRequestId = reservationRequestId;
            _visitingDateTime = visitingDateTime;
            _rejectedDateTime = rejectedDateTime;
        }   
        
        
        public Result Check()
        {
            if (_rejectedDateTime > _visitingDateTime)
            {
                return new Error($"Cannot reject pending reservation request {_reservationRequestId} " +
                                 $"which visitingDateTime is expired");
            }
            
            return Result.Success();
        }
    }
}