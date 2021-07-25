using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

namespace Reservation.Domain.ReservationRequests
{
    public sealed class ReservationRequest:Entity, IAggregateRoot
    {
        public ReservationRequestId Id { get; }
        
        private TableId _tableId;
        private ReservationRequestState _state;
        private VisitingTime _visitingTime;
        private NumberOfSeats _numberOfRequestedSeats;

        // for EF
        private ReservationRequest()
        {
            
        }
        
        private ReservationRequest(TableId tableId, VisitingTime visitingTime, NumberOfSeats numberOfRequestedSeats)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _tableId = tableId;
            _state = ReservationRequestState.Pending;
            _visitingTime = visitingTime;
            _numberOfRequestedSeats = numberOfRequestedSeats;
            
            AddDomainEvent(new ReservationIsRequestedDomainEvent(Id, _tableId, _visitingTime, _numberOfRequestedSeats));
        }

        public static Result<ReservationRequest> TryCreate(TableId tableId, VisitingTime visitingTime, NumberOfSeats numberOfRequestedSeats)
        {
            if (ContainsNullValues(new {tableId}, out var errors))
                return errors;

            return new ReservationRequest(tableId, visitingTime, numberOfRequestedSeats);
        }
    }
}