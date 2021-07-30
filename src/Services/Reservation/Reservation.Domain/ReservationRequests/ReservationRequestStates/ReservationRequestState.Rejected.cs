#region

using Ardalis.SmartEnum;

#endregion

namespace Reservation.Domain.ReservationRequests.ReservationRequestStates
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class RejectedReservationRequestState : ReservationRequestState
        {
            internal RejectedReservationRequestState()
                : base("Rejected", 4)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState)
                => false;
        }
    }
}