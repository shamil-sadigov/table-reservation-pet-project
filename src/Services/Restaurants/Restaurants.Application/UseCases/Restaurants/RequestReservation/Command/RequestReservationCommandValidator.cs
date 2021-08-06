using FluentValidation;

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.Command
{
    public class RequestReservationCommandValidator : AbstractValidator<RequestReservationCommand>
    {
        public RequestReservationCommandValidator()
        {
            RuleFor(x => x.NumberOfRequestedSeats)
                .GreaterThan((byte) 0);
        }
    }
}