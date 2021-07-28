#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using BuildingBlocks.Domain.DomainRules.SyncVersion;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.DomainRules;
using Reservation.Domain.ReservationRequests.ReservationRequestStates;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public class ReservationRequest : Entity, IAggregateRoot
    {
        private readonly NumberOfSeats _numberOfRequestedSeats;
        private readonly TableId _tableId;
        private readonly VisitorId _visitorId;
        private readonly DateTime _visitingDateTime;
        private readonly DateTime _createdDateTime;
        private ReservationRequestState _state;
        
        // for EF
        private ReservationRequest()
        {
        }

        private ReservationRequest(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            DateTime visitingDateTime,
            VisitorId visitorId,
            DateTime createdDateTime)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _tableId = tableId;
            _visitorId = visitorId;
            _numberOfRequestedSeats = numberOfRequestedSeats;
            _visitingDateTime = visitingDateTime;
            
            _createdDateTime = createdDateTime;
            _state = ReservationRequestState.Pending;
            
            AddDomainEvent(new ReservationIsRequestedDomainEvent(
                Id,
                _tableId,
                _numberOfRequestedSeats,
                _visitingDateTime,
                _visitorId));
        }

        public ReservationRequestId Id { get; }

        public static Result<ReservationRequest> TryCreate(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            VisitingTime visitingTime,
            VisitorId visitorId,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {tableId, numberOfRequestedSeats, visitingTime, visitorId, systemTime},
                out var errors))
            {
                return errors;
            }

            var visitingDateTIme = systemTime.DateNow.Add(visitingTime.AsTimeSpan());
            
            return new ReservationRequest(tableId,
                numberOfRequestedSeats,
                visitingDateTIme,
                visitorId,
                systemTime.DateTimeNow);
        }
        
        public Result<ReservationRequestApproval> TryApprove(
            AdministratorId administratorId,
            DateTime approvedDateTime,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {administratorId}, out var errors))
                return errors;
            
            var rule = new ApprovedDateTimeMustNotPassVisitingDateTimeRule(Id, _visitingDateTime, approvedDateTime)
                .And(new ReservationRequestMustBePendingToBeApprovedRule(Id, _state));
                
            var result = rule.Check();

            var switchResult = _state.TrySwitchTo(ReservationRequestState.Approved);
            
            if (switchResult.Failed)
                return switchResult.WithoutValue<ReservationRequestApproval>();

            _state = switchResult.Value!;
            
            if (result.Failed)
                return result.WithoutValue<ReservationRequestApproval>();
            
            return ReservationRequestApproval.TryCreate(Id, administratorId, approvedDateTime, systemTime);
        }
    }
}