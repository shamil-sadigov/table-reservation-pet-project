using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;
using Reservation.Domain.Tables;

namespace Reservation.Domain.Restaurants.DomainEvents
{
    public sealed record TableAddedToRestaurantDomainEvent(
        RestaurantId RestaurantId,
        TableId TableId,
        NumberOfSeats NumberOfSeats) : DomainEventBase;
}