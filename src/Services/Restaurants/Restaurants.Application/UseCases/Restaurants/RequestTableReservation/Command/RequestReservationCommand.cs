#region

using System;
using MediatR;
using Restaurants.Application.CommandContract;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestTableReservation.Command
{
    public record RequestReservationCommand(
            Guid RestaurantId,
            TimeSpan VisitingTime,
            byte NumberOfRequestedSeats)
        : ICommand<Unit>;
}