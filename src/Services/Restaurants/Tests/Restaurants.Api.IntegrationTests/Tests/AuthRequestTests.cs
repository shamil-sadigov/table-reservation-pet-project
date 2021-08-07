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
    public class AuthRequestTests : TestBase, IClassFixture<RestaurantsWebApplicationFactory>
    {
        private readonly ITestOutputHelper _output;

        public AuthRequestTests(RestaurantsWebApplicationFactory restaurantApi, ITestOutputHelper output) 
        : base(restaurantApi)
        {
            _output = output;
        }

        [Fact]
        public async Task Cannot_Post_reservation_request_when_user_is_not_authenticated()
        {
            // Arrange
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);
            
            var correlationId = Guid.NewGuid();
            
            var httpClient = RestaurantApi
                                .WithPredefinedData(restaurantSeeder)
                                .WithAvailableEventBus()
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
            response.StatusCode
                .Should().Be(HttpStatusCode.Unauthorized);
        }
        
        
        [Fact]
        public async Task Cannot_Post_reservation_request_when_user_is_not_authorized()
        {
            // Arrange
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);
            
            var correlationId = Guid.NewGuid();
            
            var httpClient = RestaurantApi
                                .WithPredefinedData(restaurantSeeder)
                                .WithAvailableEventBus()
                                .WithUnauthorizedUser(userId: Guid.NewGuid()) // <-
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
            response.StatusCode
                .Should().Be(HttpStatusCode.Forbidden);
        }
    }
}