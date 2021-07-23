using Ardalis.SmartEnum;

namespace Reservation.Domain.ReservationRequests
{
    // TODO: Add tests 
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class RejectedReservationRequestState:ReservationRequestState
        {
            public RejectedReservationRequestState()
                : base("Rejected", 4)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState) 
                => false;
        }
    }
}