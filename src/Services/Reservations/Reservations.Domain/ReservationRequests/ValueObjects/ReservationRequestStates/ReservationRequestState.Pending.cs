#region

using System;
using Ardalis.SmartEnum;

#endregion

namespace Reservations.Domain.ReservationRequests.ValueObjects.ReservationRequestStates
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        private sealed class PendingReservationRequestState : ReservationRequestState
        {
            internal PendingReservationRequestState()
                : base("Pending", 3)
            {
            }

            public override bool CanSwitchTo(ReservationRequestState nextState)
                => nextState switch
                {
                    ApprovedReservationRequestState _ => true,
                    RejectedReservationRequestState _ => true,
                    _ => throw new ArgumentOutOfRangeException(nameof(nextState), "Unexpected state")
                };
        }
    }
}