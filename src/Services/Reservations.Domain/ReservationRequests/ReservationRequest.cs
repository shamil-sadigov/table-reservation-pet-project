#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequestRejections;
using Reservations.Domain.ReservationRequests.DomainEvents;
using Reservations.Domain.ReservationRequests.DomainRules;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.ReservationRequestStates;
using Reservations.Domain.Reservations;

#endregion

namespace Reservations.Domain.ReservationRequests
{
    public class ReservationRequest : Entity, IAggregateRoot
    {
        /// <summary>
        ///     When Reservation is created
        /// </summary>
        private readonly DateTime _createdDateTime;

        private readonly RestaurantId _restaurantId;
        private readonly TableId _tableId;
        private readonly DateTime _visitingDateTime;
        private readonly VisitorId _visitorId;

        /// <summary>
        ///     When reservation is approved or rejected
        /// </summary>
        private DateTime? _closedDateTime;

        private ReservationRequestState _state;

        // for EF
        private ReservationRequest()
        {
        }

        private ReservationRequest(
            RestaurantId restaurantId,
            TableId tableId,
            VisitorId visitorId,
            DateTime visitingDateTime)
        {
            Id = new ReservationRequestId(Guid.NewGuid());
            _restaurantId = restaurantId;
            _tableId = tableId;
            _visitorId = visitorId;

            _visitingDateTime = visitingDateTime;
            _createdDateTime = SystemClock.DateTimeNow;

            _state = ReservationRequestState.Pending;

            AddDomainEvent(new ReservationRequestIsCreatedDomainEvent(
                Id,
                _restaurantId,
                _tableId,
                _createdDateTime,
                _visitingDateTime,
                _visitorId));
        }

        public ReservationRequestId Id { get; }

        public static Result<ReservationRequest> TryCreate(
            RestaurantId restaurantId,
            TableId tableId,
            VisitorId visitorId,
            DateTime visitingDateTime)
        {
            if (ContainsNullValues(new {restaurantId, tableId, visitorId},
                out var errors))
                return errors;

            return new ReservationRequest(
                restaurantId,
                tableId,
                visitorId,
                visitingDateTime);
        }

        public Result<Reservation> TryApprove(AdministratorId administratorId)
        {
            if (ContainsNullValues(new {administratorId}, out var errors))
                return errors;

            var ruleResult = new VisitingTimeMustNotPassRule(_visitingDateTime)
                .Check();

            var switchResult = _state.TrySwitchTo(ReservationRequestState.Approved);

            var combinedResult = ruleResult.CombineWith(switchResult);

            if (combinedResult.Failed)
                return combinedResult.WithoutValue<Reservation>();

            _state = switchResult.Value!;

            return Reservation.TryCreate(Id, administratorId, _restaurantId, _tableId, _visitorId);
        }

        public Result<ReservationRequestRejection> TryReject(
            AdministratorId administratorId,
            RejectionReason reason)
        {
            if (ContainsNullValues(new {administratorId, reason}, out var errors))
                return errors;

            var ruleResult = new VisitingTimeMustNotPassRule(_visitingDateTime)
                .Check();

            var switchResult = _state.TrySwitchTo(ReservationRequestState.Rejected);

            var combinedResult = ruleResult.CombineWith(switchResult);

            if (combinedResult.Failed)
                return combinedResult.WithoutValue<ReservationRequestRejection>();

            _state = switchResult.Value!;
            _closedDateTime = SystemClock.DateTimeNow;

            return ReservationRequestRejection.Create(Id, administratorId, reason);
        }
    }
}