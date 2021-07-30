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
    public class Reservation : Entity, IAggregateRoot
    {
        private readonly AdministratorId _approvedByAdministratorId;
        private readonly DateTime _approvedDateTime;

        private readonly ReservationRequestId _reservationRequestId;
        // public  ReservationRequest ReservationRequest;


        private Reservation()
        {
        }

        private Reservation(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            DateTime approvedDateTime)
        {
            // _reservationRequestId = reservationRequestId;
            _reservationRequestId = reservationRequestId;
            _approvedByAdministratorId = approvedByAdministratorId;
            _approvedDateTime = approvedDateTime;

            Id = new ReservationId(Guid.NewGuid());

            // TODO: Add here id of approval and and maybe visiting time
            AddDomainEvent(new ReservationIsMade(
                _reservationRequestId,
                _approvedByAdministratorId,
                _approvedDateTime));
        }

        public ReservationId Id { get; }

        internal static Result<Reservation> TryCreate(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            DateTime approvedDateTime,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {reservationRequestId, approvedByAdministratorId, systemTime},
                out var errors))
                return errors;

            var rule = new ApprovalDateTimeMustNotBeFutureDateRule(approvedDateTime, systemTime);

            var result = rule.Check();

            if (result.Failed)
                return result.WithoutValue<Reservation>();

            return new Reservation(
                reservationRequestId,
                approvedByAdministratorId,
                approvedDateTime);
        }
    }
}