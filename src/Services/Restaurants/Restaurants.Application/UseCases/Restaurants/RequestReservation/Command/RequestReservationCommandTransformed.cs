using System;
using Restaurants.Domain.Restaurants.ValueObjects;
using Restaurants.Domain.Tables.ValueObjects;
using Restaurants.Domain.Visitors.ValueObjects;

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.Command
{
    // Just a proxy class that for the sake of convenience converts primitive types to value objects.
    
    public class RequestReservationCommandTransformed
    {
        private readonly RequestReservationCommand _command;
        private readonly IIdentityProvider _identityProvider;

        public RequestReservationCommandTransformed(
            RequestReservationCommand command, 
            IIdentityProvider identityProvider)
        {
            _command = command;
            _identityProvider = identityProvider;
        }
        
        public VisitingTime VisitingTime => new(_command.VisitingTime);
        public VisitorId VisitorId => new(_identityProvider.UserId);
        public RestaurantId RestaurantId => new (_command.RestaurantId);
        
        public NumberOfSeats NumberOfRequestedSeats
            => NumberOfSeats.TryCreate(_command.NumberOfRequestedSeats).Value!;
    }
}