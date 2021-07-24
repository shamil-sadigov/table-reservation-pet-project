#region

using BuildingBlocks.Domain.DomainEvents;
using Reservation.Domain.Restaurants.ValueObjects;

#endregion

namespace Reservation.Domain.Restaurants.DomainEvents
{
    public sealed record NewRestaurantCreatedDomainEvent(
        RestaurantId RestaurantId,
        string Name,
        RestaurantWorkingHours RestaurantWorkingHours,
        RestaurantAddress RestaurantAddress) : DomainEventBase;
}