using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Auth;
using Restaurants.Api.Dto;
using Restaurants.Application.UseCases.Restaurants.RequestTableReservation.Command;

namespace Restaurants.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [AuthorizeRestaurantScope]
    public class RestaurantsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RestaurantsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [Route("reservation-requests")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReservationRequests([FromBody] RequestReservationRequest request)
        {
            // TODO: Handling of command can throw exception.
            // We should create an ErrorController that 
            // will catch all exceptions and map them to appropriate ProblemDetails and status code
            await _mediator.Send(new RequestReservationCommand
            (
                request.RestaurantId,
                new TimeSpan(request.VisitingTime.Hours, request.VisitingTime.Minutes, 0),
                request.NumberOfRequestedSeats
            ));
            
            return Accepted();
        }
    }
}