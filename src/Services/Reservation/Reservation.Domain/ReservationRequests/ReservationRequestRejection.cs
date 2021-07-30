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
    public class ReservationRequestRejection : Entity, IAggregateRoot
    {
        private readonly string _reason;
        private readonly AdministratorId _rejectedByAdministratorId;
        private readonly DateTime _rejectionDateTime;
        private readonly ReservationRequestId _reservationRequestId;

        private ReservationRequestRejection(
            ReservationRequestId reservationRequestId,
            AdministratorId rejectedByAdministratorId,
            DateTime rejectionDateTime,
            string reason)
        {
            _reservationRequestId = reservationRequestId;
            _rejectedByAdministratorId = rejectedByAdministratorId;
            _rejectionDateTime = rejectionDateTime;
            _reason = reason;

            Id = new ReservationRequestRejectionId(Guid.NewGuid());

            // TODO: add reservatin request rejection id here
            AddDomainEvent(new ReservationRequestIsRejected(
                _reservationRequestId,
                _rejectedByAdministratorId,
                _rejectionDateTime,
                reason));
        }

        public ReservationRequestRejectionId Id { get; }

        internal static Result<ReservationRequestRejection> TryCreate(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            DateTime approvedDateTime,
            string reason,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {reservationRequestId, approvedByAdministratorId, systemTime, reason},
                out var errors))
                return errors;

            var rule = new RejectionDateTimeMustNotBeFutureDateRule(approvedDateTime, systemTime);

            var result = rule.Check();

            if (result.Failed)
                return result.WithoutValue<ReservationRequestRejection>();

            return new ReservationRequestRejection(
                reservationRequestId,
                approvedByAdministratorId,
                approvedDateTime,
                reason);
        }
    }
}