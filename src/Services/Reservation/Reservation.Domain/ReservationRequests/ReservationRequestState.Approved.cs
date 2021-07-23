using Ardalis.SmartEnum;

namespace Reservation.Domain.ReservationRequests
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class ApprovedReservationRequestState:ReservationRequestState
        {
            public ApprovedReservationRequestState()
                : base("Approved", 1)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState) 
                => nextState switch 
                {
                    CanceledByCustomerReservationRequestState _ => true,
                    _ => false
                };
        }
    }

}