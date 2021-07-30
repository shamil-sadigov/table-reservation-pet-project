#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
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
        private readonly DateTime _createdDateTime;
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
                return errors;

            var visitingDateTIme = systemTime.DateNow.Add(visitingTime.AsTimeSpan());

            return new ReservationRequest(tableId,
                numberOfRequestedSeats,
                visitingDateTIme,
                visitorId,
                systemTime.DateTimeNow);
        }

        public Result<Reservation> TryApprove(
            AdministratorId administratorId,
            DateTime rejectionDateTime,
            ISystemTime systemTime)
        {
            if (ContainsNullValues(new {administratorId, systemTime}, out var errors))
                return errors;

            var checkResult =
                new ApprovalDateTimeMustNotPassVisitingDateTimeRule(Id, _visitingDateTime, rejectionDateTime)
                    .Check();

            var switchResult = _state.TrySwitchTo(ReservationRequestState.Approved);

            var combinedResult = checkResult.CombineWith(switchResult);

            if (combinedResult.Failed)
                return switchResult.WithoutValue<Reservation>();

            _state = switchResult.Value!;

            return Reservation.TryCreate(Id, administratorId, rejectionDateTime, systemTime);
        }

        public Result<ReservationRequestRejection> TryReject(
            AdministratorId administratorId,
            DateTime rejectionDateTime,
            ISystemTime systemTime,
            string reason)
        {
            if (ContainsNullValues(new {administratorId, systemTime}, out var errors))
                return errors;

            var ruleResult =
                new RejectionDateTimeMustNotPassVisitingDateTimeRule(Id, _visitingDateTime, rejectionDateTime)
                    .Check();

            var switchResult = _state.TrySwitchTo(ReservationRequestState.Rejected);

            var combinedResult = ruleResult.CombineWith(switchResult);

            if (combinedResult.Failed)
                return switchResult.WithoutValue<ReservationRequestRejection>();

            _state = switchResult.Value!;

            return ReservationRequestRejection.TryCreate(Id, administratorId, rejectionDateTime, reason, systemTime);
        }
    }
}