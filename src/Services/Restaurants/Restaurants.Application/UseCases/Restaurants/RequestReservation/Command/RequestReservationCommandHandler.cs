#region

using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain.DomainRules;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Restaurants;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Domain.Restaurants.ValueObjects;

#endregion

namespace Restaurants.Application.UseCases.Restaurants.RequestReservation.Command
{
    public class RequestReservationCommandHandler : IRequestHandler<RequestReservationCommand>
    {
        private readonly IIdentityProvider _identityProvider;
        private readonly ILogger<RequestReservationCommandHandler> _logger;
        private readonly IRestaurantRepository _restaurantRepository;

        public RequestReservationCommandHandler(
            ILogger<RequestReservationCommandHandler> logger,
            IRestaurantRepository restaurantRepository,
            IIdentityProvider identityProvider)
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _identityProvider = identityProvider;
        }

        // TODO: Add logging
        public async Task<Unit> Handle(RequestReservationCommand request, CancellationToken cancellationToken)
        {
            var command = new RequestReservationCommandTransformed(request, _identityProvider);

            var restaurant = await _restaurantRepository.GetAsync(command.RestaurantId);

            if (restaurant is null)
                throw new EntityNotFoundException<RestaurantId>(command.RestaurantId, nameof(Restaurant));

            Result requestResult = restaurant!.TryRequestReservation(
                command.NumberOfRequestedSeats,
                command.VisitingTime,
                command.VisitorId);

            if (requestResult.Failed)
                // TODO: Either return Exception that indicated domain logic violation like DomainLogicException
                // ot return some kind of result response instead of throwing exception
                throw new InvalidOperationException(requestResult.Errors!.Stringify());

            return Unit.Value;
        }
    }
}