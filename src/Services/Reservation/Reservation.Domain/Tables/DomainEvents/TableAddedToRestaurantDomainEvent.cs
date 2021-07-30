﻿#region

using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables.ValueObjects;

#endregion

namespace Reservation.Domain.Tables.DomainEvents
{
    public sealed record TableAddedToRestaurantDomainEvent(
        RestaurantId RestaurantId,
        TableId TableId,
        NumberOfSeats NumberOfSeats) : DomainEventBase;
}