using System;
using System.Security.Claims;
using Restaurants.Api.IntegrationTests.Helpers;

namespace Restaurants.Api.IntegrationTests.Auth
{
    public static class AuthenticationDependencyExtensions
    {
        public static RestaurantsWebApplicationFactory WithAuthenticatedUser(
            this RestaurantsWebApplicationFactory factory, 
            Guid userId)
        {
            var claims = new Claim[]
            {
                new("sub", userId.ToString()),
                new("scope", "restaurant-api")
            };

            factory.ConfigureServices += services => services.AddTestAuthentication(claims);
            return factory;
        }

        public static RestaurantsWebApplicationFactory WithUnauthorizedUser(
            this RestaurantsWebApplicationFactory factory, 
            Guid userId)
        {
            var claims = new Claim[]
            {
                new("sub", userId.ToString()),
                new("scope", "other api")
            };

            factory.ConfigureServices += services => services.AddTestAuthentication(claims);
            return factory;
        }
    }
}