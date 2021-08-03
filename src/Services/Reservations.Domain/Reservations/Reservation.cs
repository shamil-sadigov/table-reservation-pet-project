#region

using System;
using BuildingBlocks.Domain;
using BuildingBlocks.Domain.DomainRules;
using Reservations.Domain.Administrator;
using Reservations.Domain.ReservationRequests.ValueObjects;
using Reservations.Domain.Reservations.DomainEvents;

#endregion

namespace Reservations.Domain.Reservations
{
    // TODO: Add use case when reservation can be canceled
    public class Reservation : Entity, IAggregateRoot
    {
        private readonly AdministratorId _approvedByAdministratorId;
        private readonly RestaurantId _restaurantId;
        private readonly TableId _tableId;
        private readonly VisitorId _visitorId;
        private readonly DateTime _approvedDateTime;
        private readonly ReservationRequestId _reservationRequestId;

        // For EF
        private Reservation()
        {
        }

        private Reservation(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            RestaurantId restaurantId,
            TableId tableId,
            VisitorId visitorId)
        {
            Id = new ReservationId(Guid.NewGuid());

            _reservationRequestId = reservationRequestId;
            _approvedByAdministratorId = approvedByAdministratorId;
            _restaurantId = restaurantId;
            _tableId = tableId;
            _visitorId = visitorId;
            
            _approvedDateTime = SystemClock.DateTimeNow;

            // TODO: raise all props
            AddDomainEvent(new ReservationIsCreatedDomainEvent(
                Id,
                _reservationRequestId,
                _approvedByAdministratorId,
                _restaurantId,
                _tableId,
                _visitorId,
                _approvedDateTime));
        }

        public ReservationId Id { get; }

        internal static Result<Reservation> TryCreate(
            ReservationRequestId reservationRequestId,
            AdministratorId approvedByAdministratorId,
            RestaurantId restaurantId,
            TableId tableId,
            VisitorId visitorId) =>
            new Reservation(
                reservationRequestId,
                approvedByAdministratorId,
                restaurantId,
                tableId,
                visitorId);
    }
}