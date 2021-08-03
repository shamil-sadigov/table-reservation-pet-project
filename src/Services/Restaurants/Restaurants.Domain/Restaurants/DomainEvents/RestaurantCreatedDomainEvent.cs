#region

using BuildingBlocks.Domain.DomainEvents;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Domain.Restaurants.DomainEvents
{
    public sealed record RestaurantCreatedDomainEvent(
        RestaurantId RestaurantId,
        string Name,
        RestaurantWorkingHours RestaurantWorkingHours,
        RestaurantAddress RestaurantAddress) : DomainEventBase;
}