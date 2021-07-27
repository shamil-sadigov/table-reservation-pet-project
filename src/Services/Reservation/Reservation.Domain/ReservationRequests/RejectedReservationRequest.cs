#region

using System;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.DomainRules;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public partial class ReservationRequestBase
    {
        public class RejectedReservationRequest : ReservationRequestBase
        {
            private readonly AdministratorId _rejectedByAdministratorId;
            private readonly DateTime _rejectedDateTime;
            private readonly string _rejectionReason;

            public RejectedReservationRequest(
                PendingReservationRequest request,
                AdministratorId rejectedByAdministratorId,
                DateTime rejectedDateTime, 
                string rejectionReason) :
                base(
                    request.Id,
                    request.TableId,
                    request.NumberOfRequestedSeats,
                    request.VisitingDateTime,
                    request.VisitorId,
                    request.CreatedDateTime)
            {
                _rejectedByAdministratorId = rejectedByAdministratorId;
                _rejectedDateTime = rejectedDateTime;
                _rejectionReason = rejectionReason;
                
                AddDomainEvent(new ReservationRequestIsRejected(
                    Id,
                    rejectedByAdministratorId,
                    rejectedDateTime,
                    rejectionReason));
                
            }
            
            public static Result<RejectedReservationRequest> TryReject(
                PendingReservationRequest pendingReservationRequest,
                AdministratorId rejectedByAdministratorId,
                DateTime rejectedDateTime,
                string rejectionReason)
            {
                if (ContainsNullValues(new {pendingReservationRequest, rejectedByAdministratorId, rejectionReason},
                    out var errors))
                {
                    return errors;
                }

                var rule = new RejectedDateTimeMustNotPassVisitingDateTimeRule(
                    pendingReservationRequest.Id,
                    pendingReservationRequest.VisitingDateTime,
                    rejectedDateTime);

                var result = rule.Check();

                if (result.Failed) 
                    return result.WithoutValue<RejectedReservationRequest>();

                return new RejectedReservationRequest(
                    pendingReservationRequest,
                    rejectedByAdministratorId,
                    rejectedDateTime,
                    rejectionReason);
            }
        }
    }
}