#region

using BuildingBlocks.Domain.DomainEvents;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables.ValueObjects;

#endregion

namespace Restaurants.Domain.Tables.DomainEvents
{
    public sealed record TableAddedToRestaurantDomainEvent(
        RestaurantId RestaurantId,
        TableId TableId,
        NumberOfSeats NumberOfSeats) : DomainEventBase;
}