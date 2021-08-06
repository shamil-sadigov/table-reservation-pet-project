using System;

namespace Restaurants.Api.Dto
{
    public record RequestReservationRequest(
        Guid RestaurantId,
        TimeSpan VisitingTime,
        byte NumberOfRequestedSeats);
}