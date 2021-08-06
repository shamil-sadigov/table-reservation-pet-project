using Microsoft.AspNetCore.Mvc;
using Restaurants.Api.Auth;

namespace Restaurants.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [AuthorizeRestaurantScope]
    public class RestaurantController : ControllerBase
    {
        
        
        [HttpGet]
        public OkResult Get()
        {
            return Ok();
        }
    }
}