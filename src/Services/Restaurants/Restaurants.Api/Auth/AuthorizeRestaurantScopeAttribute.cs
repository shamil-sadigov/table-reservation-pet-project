using Microsoft.AspNetCore.Authorization;

namespace Restaurants.Api.Auth
{
    public class AuthorizeRestaurantScopeAttribute:AuthorizeAttribute
    {
        public AuthorizeRestaurantScopeAttribute()
            :base("restaurant-api-policy")
        {
            
        }
    }
}