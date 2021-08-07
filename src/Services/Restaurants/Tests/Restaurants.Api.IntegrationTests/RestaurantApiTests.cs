#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EventBus.RabbitMq;
using EventBus.RabbitMq.Database;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Tests.Shared;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Restaurants.Api.IntegrationTests
{
    public class RestaurantApiTests : IClassFixture<RestaurantsWebApplicationFactory>
    {
        private readonly RestaurantsWebApplicationFactoryBuilder _restaurantApi;
        private readonly ITestOutputHelper _output;

        public RestaurantApiTests(RestaurantsWebApplicationFactoryBuilder restaurantApi, ITestOutputHelper output)
        {
            _restaurantApi = restaurantApi;
            _output = output;
        }

        [Fact]
        public async Task Post_reservation_request_is_successful()
        {
            // Arrange
            var correlationId = Guid.NewGuid();
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);

            var httpClient = _restaurantApi
                                .WithPredefinedData(restaurantSeeder)
                                .WithAvailableEventBus()
                                .WithAuthenticatedUser(userId: Guid.NewGuid())
                                .CreateDefaultClient
                                (
                                    new Uri(RestaurantApi.BaseUrl),
                                    new CorrelationIdProvider(correlationId)
                                );
            // Act
            var response = await httpClient.PostAsJsonAsync(RestaurantApi.ReservationRequestsPath, new
            {
                // TODO: extract anonymous object to dto record
                
                RestaurantId = restaurant.Id.Value,
                VisitingTime = new
                {
                    Hours = 12,
                    Minutes = 00
                },
                NumberOfRequestedSeats = 2
            });

            // Assert            
            if (!response.IsSuccessStatusCode) 
                _output.WriteLine(await response.Content.ReadAsStringAsync());
            
            response.StatusCode
                .Should().Be(HttpStatusCode.Accepted);

             GetPublishedIntegrationEvents(correlationId)
                .Should()
                .ContainSingle();
             
            // Also assert that command has been saved with the same correlation id
        }

        private IReadOnlyCollection<IntegrationEventEntry> GetPublishedIntegrationEvents(Guid correlationId)
        {
            return _restaurantApi.Services
                .GetRequiredService<IntegrationEventContext>()
                .IntegrationEvents
                .Where(x=> x.CorrelationId == correlationId)
                .Where(x=> x.State == IntegrationEventState.Published)
                .ToList();
        }

        // TODO: Add tests that will fail
        // - Without correlation Id
        // - 2 requests with the same correlation Id
        
        
        private static async Task<Domain.Restaurants.Restaurant> CreateSingleRestaurant()
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
    }
}