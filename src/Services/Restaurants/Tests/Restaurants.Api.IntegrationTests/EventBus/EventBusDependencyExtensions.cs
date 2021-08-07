using System;
using System.Security.Claims;
using EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Restaurants.Api.IntegrationTests.EventBus
{
    public static class EventBusDependencyExtensions
    {
        public static RestaurantsWebApplicationFactory WithAvailableEventBus(
            this RestaurantsWebApplicationFactory factory)
        {
            factory.ConfigureServices += services => services.AddScoped<IEventBus, AvailableEventBus>();
            return factory;
        }
        
        
        public static RestaurantsWebApplicationFactory WithUnavailableEventBus(
            this RestaurantsWebApplicationFactory factory)
        {
            factory.ConfigureServices += services => services.AddScoped<IEventBus, UnAvailableEventBus>();
            return factory;
        }
    }
}