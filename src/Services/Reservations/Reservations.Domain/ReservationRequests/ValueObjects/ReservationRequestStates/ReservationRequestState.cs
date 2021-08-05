#region

using Ardalis.SmartEnum;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;

#endregion

// TODO: Add canceled by visitor state

namespace Reservations.Domain.ReservationRequests.ValueObjects.ReservationRequestStates
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        public static readonly ReservationRequestState Pending = new PendingReservationRequestState();
        public static readonly ReservationRequestState Approved = new ApprovedReservationRequestState();
        public static readonly ReservationRequestState Rejected = new RejectedReservationRequestState();

        private ReservationRequestState(string name, int value) : base(name, value)
        {
        }

        public Result<ReservationRequestState> TrySwitchTo(ReservationRequestState nextState)
        {
            if (CanSwitchTo(nextState))
                return nextState;

            return new Error($"Cannot switch from '{Name}' state to '{nextState.Name}' state");
        }

        // TODO: make protected
        public abstract bool CanSwitchTo(ReservationRequestState nextState);
    }
}