#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public abstract partial class ReservationRequestBase : Entity, IAggregateRoot
    {
        protected readonly NumberOfSeats NumberOfRequestedSeats;
        protected readonly TableId TableId;
        protected readonly DateTime VisitingDateTime;
        protected readonly VisitorId VisitorId;
        protected readonly DateTime CreatedDateTime;
        
        // for EF
        private ReservationRequestBase()
        {
        }

        protected ReservationRequestBase(
            ReservationRequestId id,
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            DateTime visitingDateTime,
            VisitorId visitorId,
            DateTime createdDateTime)
        {
            Id = id;
            TableId = tableId;
            VisitorId = visitorId;
            NumberOfRequestedSeats = numberOfRequestedSeats;
            VisitingDateTime = visitingDateTime;
            
            CreatedDateTime = createdDateTime;
        }

        public ReservationRequestId Id { get; }

        public static Result<PendingReservationRequest> TryCreatePending(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            VisitingTime visitingTime,
            VisitorId visitorId,
            ISystemTime systemTime)
        {
            return PendingReservationRequest.TryCreate(
                tableId, numberOfRequestedSeats, visitingTime, visitorId, systemTime);
        }
    }
}