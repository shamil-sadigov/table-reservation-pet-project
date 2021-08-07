using System;
using System.Linq;
using System.Threading.Tasks;
using EventBus.RabbitMq.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Tests.Shared;
using Restaurants.Application;
using Restaurants.Infrastructure.Contexts;

namespace Restaurants.Api.IntegrationTests.Tests
{
    public abstract class TestBase
    {
        protected readonly RestaurantsWebApplicationFactory RestaurantApi;

        protected TestBase(RestaurantsWebApplicationFactory restaurantApi)
        {
            RestaurantApi = restaurantApi;
        }
        
        protected static async Task<Domain.Restaurants.Restaurant> CreateSingleRestaurant()
        {
            var restaurant = await RestaurantBuilder
                .WithName("restaurant name")
                .LocatingAt("restaurant address")
                .WithWorkingSchedule("09:00", "22:00")
                .WithTables(
                    ("TBL-1", numberOfSeats: 2),
                    ("TBL-2", numberOfSeats: 4),
                    ("TBL-3", numberOfSeats: 2),
                    ("TBL-4", numberOfSeats: 8))
                .BuildAsync();
            return restaurant;
        }
        
        protected Task<IntegrationEventEntry> GetIntegrationEventAsync(Guid correlationId)
        {
            return RestaurantApi.Services
                .GetRequiredService<IntegrationEventContext>()
                .IntegrationEvents
                .Where(x=> x.CorrelationId == correlationId)
                .SingleOrDefaultAsync();
        }
        
        protected async Task<Command> GetExecutedCommandAsync(Guid correlationId)
        {
            return await RestaurantApi.Services
                .GetRequiredService<RestaurantContext>()
                .Commands
                .Where(x=> x.CorrelationId == correlationId)
                .SingleOrDefaultAsync();
        }
    }
}