#region

using FluentValidation;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestTableReservation.Command
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