using FluentAssertions;
using Reservation.Domain.ReservationRequests;
using Xunit;

namespace Reservation.Domain.Tests.ReservationRequests
{
    public class ReservationRequestStateTests
    {
        [Theory]
        [InlineData("Approved")]
        [InlineData("Rejected")]
        [InlineData("CanceledByCustomer")]
        public void Can_switch_from_pending_state_to_expected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            ReservationRequestState.Pending.CanSwitchTo(nextState)
                .Should()
                .BeTrue();
        }
        
        [Theory]
        [InlineData("CanceledByCustomer")]
        public void Can_switch_from_approved_state_to_expected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            ReservationRequestState.Approved.CanSwitchTo(nextState)
                .Should()
                .BeTrue();
        }
        
        [Theory]
        [InlineData("Rejected")]
        [InlineData("Approved")]
        [InlineData("Pending")]
        public void Cannot_switch_from_approved_state_to_unexpected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            ReservationRequestState.Approved.CanSwitchTo(nextState)
                .Should()
                .BeFalse();
        }
        [Theory]
        [InlineData("Approved")]
        [InlineData("Pending")]
        [InlineData("CanceledByCustomer")]
        public void Cannot_switch_from_rejected_state_to_unexpected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            ReservationRequestState.Rejected.CanSwitchTo(nextState)
                .Should()
                .BeFalse();
        }
        
        [Theory]
        [InlineData("Approved")]
        [InlineData("Rejected")]
        [InlineData("Pending")]
        public void Cannot_switch_from_canceled_state_to_unexpected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            ReservationRequestState.CanceledByCustomer.CanSwitchTo(nextState)
                .Should()
                .BeFalse();
        }
    }
}