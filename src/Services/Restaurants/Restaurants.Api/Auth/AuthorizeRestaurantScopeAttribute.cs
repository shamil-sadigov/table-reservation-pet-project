#region

using Microsoft.AspNetCore.Authorization;

#endregion

namespace Restaurants.Api.Auth
{
    public class AuthorizeRestaurantScopeAttribute : AuthorizeAttribute
    {
        public AuthorizeRestaurantScopeAttribute()
            : base("restaurant-api-policy")
        {
        }
    }
}