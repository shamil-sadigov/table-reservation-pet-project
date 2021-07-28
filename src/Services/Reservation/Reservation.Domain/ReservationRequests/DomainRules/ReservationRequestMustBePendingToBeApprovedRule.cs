using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;

namespace Reservation.Domain.ReservationRequests.DomainRules
{
    public class ReservationRequestMustBePendingToBeApprovedRule:IDomainRule
    {
        private readonly ReservationRequestId _reservationRequestId;
        private readonly ReservationRequestState _reservationRequestState;

        public ReservationRequestMustBePendingToBeApprovedRule(
            ReservationRequestId reservationRequestId,
            ReservationRequestState reservationRequestState)
        {
            _reservationRequestId = reservationRequestId;
            _reservationRequestState = reservationRequestState;
        }
        
        public Result Check()
        {
            if (_reservationRequestState != ReservationRequestState.Pending)
            {
                return new Error($"Reservation request {_reservationRequestId}" +
                                 $" must be pending to be approved");
            }
            
            return Result.Success();
        }
    }
}