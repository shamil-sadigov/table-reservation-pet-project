#region

using BuildingBlocks.Domain.DomainEvents;

#endregion

namespace Reservation.Domain.Restaurants.DomainEvents
{
    public sealed record NewRestaurantRegisteredDomainEvent(
        RestaurantId RestaurantId,
        string Name,
        RestaurantWorkingHours RestaurantWorkingHours,
        RestaurantAddress RestaurantAddress) : DomainEventBase;
}