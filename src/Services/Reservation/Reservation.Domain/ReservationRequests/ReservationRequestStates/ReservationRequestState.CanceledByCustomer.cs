using Ardalis.SmartEnum;

namespace Reservation.Domain.ReservationRequests.ReservationRequestStates
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class CanceledByCustomerReservationRequestState:ReservationRequestState
        {
            public CanceledByCustomerReservationRequestState()
                : base("CanceledByCustomer", 2)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState)
                => false;
        }
    }

}