#region

using Ardalis.SmartEnum;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public abstract partial class ReservationRequestState : SmartEnum<ReservationRequestState>
    {
        public static readonly ReservationRequestState Pending = new PendingReservationRequestState();
        public  static readonly ReservationRequestState Approved = new ApprovedReservationRequestState();
        public  static readonly ReservationRequestState Rejected = new RejectedReservationRequestState();

        public  static readonly ReservationRequestState CanceledByCustomer =
            new CanceledByCustomerReservationRequestState();

        private ReservationRequestState(string name, int value) : base(name, value)
        {
        }

        public abstract bool CanSwitchTo(ReservationRequestState nextState);
    }
}