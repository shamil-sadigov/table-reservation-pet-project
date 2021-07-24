using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.Restaurants;
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

        // for EF
        private ReservationRequest()
        {
            
        }
        
        private ReservationRequest(TableId tableId, VisitingTime visitingTime)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _tableId = tableId;
            _state = ReservationRequestState.Pending;
            _visitingTime = visitingTime;
            AddDomainEvent(new ReservationIsRequestedDomainEvent(Id, _tableId));
        }

        public static Result<ReservationRequest> TryCreate(TableId tableId, VisitingTime visitingTime)
        {
            if (ContainsNullValues(new {tableId}, out var errors))
                return errors;

            return new ReservationRequest(tableId, visitingTime);
        }
    }
}