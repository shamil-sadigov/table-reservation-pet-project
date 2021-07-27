#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public sealed class ReservationRequest : Entity, IAggregateRoot
    {
        private readonly NumberOfSeats _numberOfRequestedSeats;

        private readonly TableId _tableId;
        private readonly DateTime _visitingDateTime;
        private readonly VisitorId _visitorId;
        private ReservationRequestState _state;

        // for EF
        private ReservationRequest()
        {
        }

        private ReservationRequest(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            DateTime visitingDateTime,
            VisitorId visitorId)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _tableId = tableId;
            _state = ReservationRequestState.Pending;
            _visitorId = visitorId;
            _numberOfRequestedSeats = numberOfRequestedSeats;
            _visitingDateTime = visitingDateTime;

            AddDomainEvent(new ReservationIsRequestedDomainEvent(Id,
                _tableId,
                _numberOfRequestedSeats,
                _visitingDateTime,
                visitorId));
        }

        public ReservationRequestId Id { get; }

        public static Result<ReservationRequest> TryCreate(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            VisitingTime visitingTime,
            VisitorId visitorId,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {tableId}, out var errors))
                return errors;

            var visitingDateTIme = systemTime.DateNow.Add(visitingTime.AsTimeSpan());

            return new ReservationRequest(tableId, numberOfRequestedSeats, visitingDateTIme, visitorId);
        }
    }
}