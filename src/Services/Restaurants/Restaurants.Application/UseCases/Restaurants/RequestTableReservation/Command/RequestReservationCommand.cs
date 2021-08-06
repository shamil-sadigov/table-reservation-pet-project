#region

using System;
using MediatR;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestTableReservation.Command
{
    public record RequestReservationCommand(
            Guid RestaurantId,
            TimeSpan VisitingTime,
            byte NumberOfRequestedSeats)
        : ICommand<Unit>;
}