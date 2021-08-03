#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequestRejections.DomainEvents;
using Reservations.Domain.ReservationRequestRejections.DomainRules;
using Reservations.Domain.ReservationRequests.DomainRules;
using Reservations.Domain.ReservationRequests.ValueObjects;

#endregion

namespace Reservations.Domain.ReservationRequestRejections
{
    public class ReservationRequestRejection : Entity, IAggregateRoot
    {
        private readonly RejectionReason _reason;
        private readonly AdministratorId _rejectedByAdministratorId;
        private readonly DateTime _rejectionDateTime;
        private readonly ReservationRequestId _reservationRequestId;

        private ReservationRequestRejection(
            ReservationRequestId reservationRequestId,
            AdministratorId rejectedByAdministratorId,
            RejectionReason reason)
        {
            _reservationRequestId = reservationRequestId;
            _rejectedByAdministratorId = rejectedByAdministratorId;
            _reason = reason;
            
            _rejectionDateTime = SystemClock.DateTimeNow;
            Id = new ReservationRequestRejectionId(Guid.NewGuid());

            AddDomainEvent(new ReservationRequestIsRejected(
                Id,
                _reservationRequestId,
                _rejectedByAdministratorId,
                _rejectionDateTime,
                _reason));
        }

        public ReservationRequestRejectionId Id { get; }

        internal static ReservationRequestRejection Create(
            ReservationRequestId reservationRequestId,
            AdministratorId rejectedByAdministratorId,
            RejectionReason reason) =>
            new(reservationRequestId,
                rejectedByAdministratorId,
                reason);
    }
}