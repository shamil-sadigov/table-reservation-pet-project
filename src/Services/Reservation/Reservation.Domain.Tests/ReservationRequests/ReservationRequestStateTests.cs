using FluentAssertions;
using Reservation.Domain.ReservationRequests;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.Tests.Helpers;
using Xunit;

namespace Reservation.Domain.Tests.ReservationRequests
{
    public class ReservationRequestStateTests
    {
        [Theory]
        [InlineData("Approved")]
        [InlineData("Rejected")]
        public void Can_switch_from_pending_state_to_expected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            var result = ReservationRequestState.Pending.TrySwitchTo(nextState);
            
            result.ShouldSucceed();
        }
        
        [Theory]
        [InlineData("Rejected")]
        [InlineData("Approved")]
        [InlineData("Pending")]
        public void Cannot_switch_from_approved_state_to_unexpected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            var result = ReservationRequestState.Approved.TrySwitchTo(nextState);
            
            result.ShouldFail();
        }
        
        [Theory]
        [InlineData("Approved")]
        [InlineData("Pending")]
        public void Cannot_switch_from_rejected_state_to_unexpected_state(string nextStateStr)
        {
            var nextState = ReservationRequestState.FromName(nextStateStr);

            var result = ReservationRequestState.Rejected.TrySwitchTo(nextState);
            
            result.ShouldFail();
        }
    }
}