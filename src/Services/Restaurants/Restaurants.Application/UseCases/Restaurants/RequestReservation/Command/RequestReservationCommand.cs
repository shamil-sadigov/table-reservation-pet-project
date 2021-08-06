using System;
using MediatR;
using Restaurants.Application.CommandContract;

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.Command
{
    public record RequestReservationCommand(
            Guid RestaurantId,
            TimeSpan VisitingTime,
            byte NumberOfRequestedSeats)
        : ICommand<Unit>;
}