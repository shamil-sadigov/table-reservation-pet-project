#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.DomainRules;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public partial class ReservationRequestBase
    {
        public class ApprovedReservationRequest : ReservationRequestBase
        {
            // TODO: Create Reservation in ReservationRequestApprovedDomainEvent
            
            private readonly AdministratorId _approvedByAdministratorId;
            private readonly DateTime _approvedDateTime;

            private ApprovedReservationRequest(
                PendingReservationRequest reservationRequest,
                AdministratorId approvedByAdministratorId,
                DateTime approvedDateTime)
                : base(
                    reservationRequest.Id,
                    reservationRequest.TableId,
                    reservationRequest.NumberOfRequestedSeats,
                    reservationRequest.VisitingDateTime,
                    reservationRequest.VisitorId,
                    reservationRequest.CreatedDateTime)
            {
                _approvedByAdministratorId = approvedByAdministratorId;
                _approvedDateTime = approvedDateTime;
                
                AddDomainEvent(new ReservationRequestIsApproved(Id, approvedByAdministratorId, approvedDateTime));
            }

            internal static Result<ApprovedReservationRequest> TryApprove(
                PendingReservationRequest pendingReservationRequest,
                AdministratorId approvedByAdministratorId,
                DateTime approvedDateTime)
            {
                if (ContainsNullValues(new {pendingReservationRequest, approvedByAdministratorId}, out var errors))
                    return errors;
                
                var rule = new ApprovedDateTimeMustNotPassVisitingDateTimeRule(
                    pendingReservationRequest.Id,
                    pendingReservationRequest.VisitingDateTime,
                    approvedDateTime);

                var result = rule.Check();

                if (result.Failed)
                    return result.WithoutValue<ApprovedReservationRequest>();

                return new ApprovedReservationRequest(
                    pendingReservationRequest,
                    approvedByAdministratorId, 
                    approvedDateTime);
            }

            public Reservation MakeReservation()
            {
                
            }
        }
    }

    public class Reservation:Entity, IAggregateRoot
    {
        
    }

    public class ReservationId:
    {
        
    }
}