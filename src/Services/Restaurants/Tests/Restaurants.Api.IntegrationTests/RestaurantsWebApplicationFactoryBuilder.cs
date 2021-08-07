using System;
using System.Security.Claims;
using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurants.Api.IntegrationTests
{
    public sealed class RestaurantsWebApplicationFactoryBuilder:RestaurantsWebApplicationFactory
    {
        public RestaurantsWebApplicationFactoryBuilder WithPredefinedData(IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
            return this;
        }

        public RestaurantsWebApplicationFactoryBuilder WithAuthenticatedUser(Guid userId)
        {
            var claims = new Claim[]
            {
                new("sub", userId.ToString()),
                new("scope", "restaurant-api")
            };

            ConfigureServices += services => services.AddTestAuthentication(claims);
            return this;
        }

        public RestaurantsWebApplicationFactoryBuilder WithUnauthorizedUser(Guid userId)
        {
            var claims = new Claim[]
            {
                new("sub", userId.ToString()),
                new("scope", "other scope")
            };

            ConfigureServices += services => services.AddTestAuthentication(claims);
            return this;
        }

        public RestaurantsWebApplicationFactoryBuilder WithAvailableEventBus()
        {
            ConfigureServices += services => services.AddScoped<IEventBus, AvailableEventBus>();
            return this;
        }
        
        public RestaurantsWebApplicationFactoryBuilder WithUnavailableEventBus()
        {
            ConfigureServices += services => services.AddScoped<IEventBus, UnAvailableEventBus>();
            return this;
        }
    }
}