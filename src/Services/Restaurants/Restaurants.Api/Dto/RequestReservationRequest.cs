#region

using System;

#endregion

namespace Restaurants.Api.Dto
{
    public record RequestReservationRequest(
        Guid RestaurantId,
        VisitingTime VisitingTime,
        byte NumberOfRequestedSeats);

    public record VisitingTime(byte Hours, byte Minutes);

    // It would be better for 'VisitingTime' to be of TimeSpan type
    // but unfortunately TimeSpan is not supported well in JSON
    // So, instead let's just specify hours and minutes separately
}