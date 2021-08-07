#region

using System;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EventBus.RabbitMq.Database;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurant.Tests.Shared;
using Restaurants.Api.IntegrationTests.Auth;
using Restaurants.Api.IntegrationTests.DataSeeders;
using Restaurants.Api.IntegrationTests.EventBus;
using Restaurants.Api.IntegrationTests.Helpers;
using Restaurants.Application;
using Restaurants.Infrastructure.Contexts;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Restaurants.Api.IntegrationTests.Tests
{
    public class EventBusAvailabilityTests : TestBase, IClassFixture<RestaurantsWebApplicationFactory>
    {
        private readonly RestaurantsWebApplicationFactory _restaurantApi;
        private readonly ITestOutputHelper _output;

        public EventBusAvailabilityTests(RestaurantsWebApplicationFactory restaurantApi, ITestOutputHelper output)
        :base(restaurantApi)
        {
            _restaurantApi = restaurantApi;
            _output = output;
        }
        
        [Fact]
        public async Task Post_reservation_request_is_successful()
        {
            // Arrange
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);
            
            var correlationId = Guid.NewGuid();
            
            var httpClient = _restaurantApi
                                .WithPredefinedData(restaurantSeeder)
                                .WithUnavailableEventBus()
                                .WithAuthenticatedUser(userId: Guid.NewGuid())
                                .CreateDefaultClient
                                (
                                    new Uri(IntegrationTests.RestaurantApi.BaseUrl),
                                    new CorrelationIdProvider(correlationId)
                                );
            // Act
            var response = await httpClient.PostAsJsonAsync(IntegrationTests.RestaurantApi.ReservationRequestsPath, new
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

            var integrationEvent = await GetIntegrationEventAsync(correlationId);

            integrationEvent.Should().NotBeNull();
            
            integrationEvent.ShouldBeFailedToPublish();
            
            var executedCommand = await GetExecutedCommandAsync(correlationId);
            
            executedCommand.Should().NotBeNull();

            integrationEvent.ShouldBeCausedBy(executedCommand);
        }
    }
}