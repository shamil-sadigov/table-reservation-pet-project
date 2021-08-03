#region

using Ardalis.SmartEnum;

#endregion

namespace Reservations.Domain.ReservationRequestStates
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class ApprovedReservationRequestState : ReservationRequestState
        {
            internal ApprovedReservationRequestState()
                : base("Approved", 1)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState)
                => nextState switch
                {
                    _ => false
                };
        }
    }
}