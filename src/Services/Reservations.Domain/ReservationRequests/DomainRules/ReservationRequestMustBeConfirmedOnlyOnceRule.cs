using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.ReservationRequestStates;

namespace Reservations.Domain.ReservationRequests.DomainRules
{
    public class ReservationRequestMustBeConfirmedOnlyOnceRule : IDomainRule
    {
        private readonly ReservationRequestId _reservationRequestId;
        private readonly ReservationRequestState _currentState;

        public ReservationRequestMustBeConfirmedOnlyOnceRule(
            ReservationRequestId reservationRequestId,
            ReservationRequestState currentState)
        {
            _reservationRequestId = reservationRequestId;
            _currentState = currentState;
        }
        
        public Result Check()
        {
            if (_currentState != ReservationRequestState.Pending)
            {
                return new Error($"Reservation request '{_reservationRequestId}' must be in pending state." +
                                 $"But current state is {_currentState}");
            }
            
            return Result.Success();
        }
    }
}