#region

using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Restaurants.Api.Exceptions;
using Restaurants.Api.IntegrationTests.Auth;
using Restaurants.Api.IntegrationTests.DataSeeders;
using Restaurants.Api.IntegrationTests.EventBus;
using Restaurants.Api.IntegrationTests.Helpers;
using Restaurants.Application.Exceptions;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Restaurants.Api.IntegrationTests.Tests
{
    public class CorrelationIdTests : TestBase, IClassFixture<RestaurantsWebApplicationFactory>
    {
        public CorrelationIdTests(RestaurantsWebApplicationFactory restaurantApi, ITestOutputHelper output)
            : base(restaurantApi)
        {
        }

        [Fact]
        public async Task Cannot_Post_reservation_request_when_correlationId_is_not_provided_in_request()
        {
            // Arrange
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);

            var correlationId = Guid.NewGuid();

            var httpClient = RestaurantApi
                .WithPredefinedData(restaurantSeeder)
                .WithAvailableEventBus()
                .WithAuthenticatedUser(userId: Guid.NewGuid())
                .CreateDefaultClient
                (
                    new Uri(IntegrationTests.RestaurantApi.BaseUrl)
                );
            // Act
            Func<Task> postRequest = async () =>
            {
                await httpClient.PostAsJsonAsync(IntegrationTests.RestaurantApi.ReservationRequestsPath, new
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
            };

            // Assert
            await postRequest.Should()
                .ThrowAsync<CorrelationIdException>()
                .WithMessage("Request should contains CorrelationId *");
        }

        [Fact]
        public async Task Cannot_Post_reservation_request_when_duplicate_correlationId_is_specified_in_request()
        {
            // Arrange
            var restaurant = await CreateSingleRestaurant();

            var restaurantSeeder = new RestaurantSeeder(restaurant);

            var correlationId = Guid.NewGuid();

            var httpClient = RestaurantApi
                .WithPredefinedData(restaurantSeeder)
                .WithAvailableEventBus()
                .WithAuthenticatedUser(userId: Guid.NewGuid())
                .CreateDefaultClient
                (
                    new Uri(IntegrationTests.RestaurantApi.BaseUrl),
                    new CorrelationIdProvider(correlationId)
                );

            // Act

            // First post
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

            response.IsSuccessStatusCode.Should().BeTrue();

            // second duplicated post
            Func<Task> duplicatePost = async () =>
            {
                await httpClient.PostAsJsonAsync(IntegrationTests.RestaurantApi.ReservationRequestsPath, new
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
            };

            // Assert
            await duplicatePost
                .Should().ThrowAsync<DuplicateRequestException>()
                .WithMessage($"Request with CorrelationId {correlationId} has been already sent *");
        }
    }
}