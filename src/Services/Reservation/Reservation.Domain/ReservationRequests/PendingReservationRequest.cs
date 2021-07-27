#region

using System;
using BuildingBlocks.Domain.DomainRules;
using Reservation.Domain.ReservationRequests.DomainEvents;
using Reservation.Domain.ReservationRequests.ValueObjects;
using Reservation.Domain.Tables;
using Reservation.Domain.Tables.ValueObjects;
using Reservation.Domain.Visitors.ValueObjects;

#endregion

namespace Reservation.Domain.ReservationRequests
{
    public class PendingReservationRequest : ReservationRequestBase
    {
        private PendingReservationRequest(
            TableId tableId,
            NumberOfSeats numberOfRequestedSeats,
            DateTime visitingDateTime,
            VisitorId visitorId,
            DateTime createdDateTime)
            : base(
                new ReservationRequestId(Guid.NewGuid()),
                tableId,
                numberOfRequestedSeats,
                visitingDateTime,
                visitorId,
                createdDateTime)
        {
            AddDomainEvent(new ReservationIsRequestedDomainEvent(Id,
                TableId,
                NumberOfRequestedSeats,
                VisitingDateTime,
                VisitorId));
        }

        internal static Result<PendingReservationRequest> TryCreate(
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

            return new PendingReservationRequest(
                tableId,
                numberOfRequestedSeats,
                visitingDateTIme,
                visitorId,
                systemTime.DateTimeNow);
        }

        public Result<ApprovedReservationRequest> TryApprove(
            AdministratorId administratorId,
            DateTime approvedDateTime)
        {
            return ApprovedReservationRequest.TryApprove(this, administratorId, approvedDateTime);
        }
        
        public Result<RejectedReservationRequest> TryReject(
            AdministratorId administratorId,
            DateTime rejectedDateTime,
            string reason)
        {
            return RejectedReservationRequest.TryReject(this, administratorId, rejectedDateTime, reason);
        }

        
        // TODO: Add TryCancel method
    }
}