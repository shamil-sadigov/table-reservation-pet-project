#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public sealed class ReservationRequest : Entity, IAggregateRoot
    {
        private readonly NumberOfSeats _numberOfRequestedSeats;
        private ReservationRequestState _state;

        private readonly TableId _tableId;
        private readonly VisitingTime _visitingTime;

        // for EF
        private ReservationRequest()
        {
        }

        private ReservationRequest(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            VisitingTime visitingTime,
            VisitorId visitorId)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _tableId = tableId;
            _state = ReservationRequestState.Pending;
            _visitingTime = visitingTime;
            _numberOfRequestedSeats = numberOfRequestedSeats;

            AddDomainEvent(new ReservationIsRequestedDomainEvent(Id,
                _tableId,
                _numberOfRequestedSeats,
                _visitingTime,
                visitorId));
        }

        public ReservationRequestId Id { get; }

        public static Result<ReservationRequest> TryCreate(
            TableId tableId, 
            NumberOfSeats numberOfRequestedSeats,
            VisitingTime visitingTime,
            VisitorId visitorId)
        {
            if (ContainsNullValues(new {tableId}, out var errors))
                return errors;

            return new ReservationRequest(tableId, numberOfRequestedSeats, visitingTime, visitorId);
        }
    }
}