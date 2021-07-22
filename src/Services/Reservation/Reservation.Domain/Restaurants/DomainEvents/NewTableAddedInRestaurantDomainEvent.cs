using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Tables;

namespace Reservation.Domain.Restaurants.DomainEvents
{
    public sealed record NewTableAddedInRestaurantDomainEvent(
        RestaurantId RestaurantId,
        TableId TableId,
        NumberOfSeats NumberOfSeats) : DomainEventBase;
}