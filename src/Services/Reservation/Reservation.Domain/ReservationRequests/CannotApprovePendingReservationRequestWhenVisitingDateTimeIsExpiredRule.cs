using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.ReservationRequests
{
    public class CannotApprovePendingReservationRequestWhenVisitingDateTimeIsExpiredRule:IDomainRule
    {
        private readonly ReservationRequestId _reservationRequestId;
        private readonly DateTime _visitingDateTime;
        private readonly DateTime _approvalDateTime;

        public CannotApprovePendingReservationRequestWhenVisitingDateTimeIsExpiredRule(
            ReservationRequestId reservationRequestId,
            DateTime visitingDateTime,
            DateTime approvalDateTime)
        {
            _reservationRequestId = reservationRequestId;
            _visitingDateTime = visitingDateTime;
            _approvalDateTime = approvalDateTime;
        }   
        
        
        public Result Check()
        {
            if (_approvalDateTime > _visitingDateTime)
            {
                return new Error($"Cannot approve pending reservation request {_reservationRequestId} " +
                                 $"which visitingDateTime is expired");
            }
            
            return Result.Success();
        }
    }
}