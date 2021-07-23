using Ardalis.SmartEnum;

namespace Reservation.Domain.ReservationRequests
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class PendingReservationRequestState:ReservationRequestState
        {
            public PendingReservationRequestState()
                : base("Pending", 3)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState) 
                => nextState switch
                {
                    CanceledByCustomerReservationRequestState _ => true,
                    ApprovedReservationRequestState _ => true,
                    RejectedReservationRequestState _ => true,
                    _ => false
                };
        }
    }

}