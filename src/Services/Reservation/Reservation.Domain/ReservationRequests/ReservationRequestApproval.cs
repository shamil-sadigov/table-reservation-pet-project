#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.DomainRules;
using Reservation.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public class ReservationRequestApproval : Entity, IAggregateRoot
    {
        public ReservationRequestApprovalId Id { get; }
        
        private readonly AdministratorId _approvedByAdministratorId;
        private readonly DateTime _approvedDateTime;
        
        // TODO: Create Reservation in ReservationRequestApprovedDomainEvent

        private readonly ReservationRequestId _reservationRequestId;

        private ReservationRequestApproval(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            DateTime approvedDateTime)
        {
            _reservationRequestId = reservationRequestId;
            _approvedByAdministratorId = approvedByAdministratorId;
            _approvedDateTime = approvedDateTime;

            Id = new ReservationRequestApprovalId(Guid.NewGuid());
            
            AddDomainEvent(new ReservationRequestIsApproved(
                _reservationRequestId,
                _approvedByAdministratorId,
                _approvedDateTime));
        }

        internal static Result<ReservationRequestApproval> TryCreate(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            DateTime approvedDateTime,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {reservationRequestId, approvedByAdministratorId, systemTime},
                out var errors))
            {
                return errors;
            }

            var rule = new ApprovedDateTimeMustNotBeFutureDateRule(approvedDateTime, systemTime);
            
            var result = rule.Check();
            
            if (result.Failed)
                return result.WithoutValue<ReservationRequestApproval>();
            
            return new ReservationRequestApproval(
                reservationRequestId,
                approvedByAdministratorId,
                approvedDateTime);
        }

        public Reservation MakeReservation()
        {
            throw new NotImplementedException();
        }
    }


    public class Reservation : Entity, IAggregateRoot
    {
    }
}