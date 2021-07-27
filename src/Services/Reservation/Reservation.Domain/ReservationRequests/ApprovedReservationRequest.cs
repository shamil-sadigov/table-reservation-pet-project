#region

using System;
using BuildingBlocks.Domain.DomainRules;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public partial class ReservationRequestBase
    {
        public class ApprovedReservationRequest : ReservationRequestBase
        {
            // TODO: Create Reservation in ReservationRequestApprovedDomainEvent
            
            private readonly AdministratorId _approvedByAdministratorId;
            private readonly DateTime _approvalDateTime;

            protected ApprovedReservationRequest(
                PendingReservationRequest reservationRequest,
                ISystemTime systemTime,
                AdministratorId approvedByAdministratorId,
                DateTime approvalDateTime)
                : base(
                    reservationRequest.Id,
                    reservationRequest.TableId,
                    reservationRequest.NumberOfRequestedSeats,
                    reservationRequest.VisitingDateTime,
                    reservationRequest.VisitorId,
                    systemTime)
            {
                _approvedByAdministratorId = approvedByAdministratorId;
                _approvalDateTime = approvalDateTime;
                // TODO: Add domain event here
            }

            internal static Result<ApprovedReservationRequest> TryCreateFrom(
                PendingReservationRequest pendingReservationRequest,
                AdministratorId approvedByAdministratorId,
                DateTime approvalDateTime,
                ISystemTime systemTime)
            {
                var rule = new CannotApprovePendingReservationRequestWhenVisitingDateTimeIsExpiredRule(
                    pendingReservationRequest.Id,
                    pendingReservationRequest.VisitingDateTime,
                    approvalDateTime);

                var result = rule.Check();

                if (result.Failed)
                {
                    return result.WithoutValue<ApprovedReservationRequest>();
                }

                return new ApprovedReservationRequest(
                    pendingReservationRequest, 
                    systemTime,
                    approvedByAdministratorId, 
                    approvalDateTime);
            }
        }
    }
}