using System;
using System.IO;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.Auth;
using Restaurants.Infrastructure.Contexts;

namespace Restaurants.Api.IntegrationTests
{
    public class RestaurantsWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(ops =>
            {
                // TODO: Consider to add UserSecrets as well
                ops.AddJsonFile("appsettings.testing.json");
            });

            builder.UseEnvironment("testing");
            
            builder.UseContentRoot(Directory.GetCurrentDirectory());
            
            builder.ConfigureServices(services =>
            {
                services.AddAuthentication("Test-Scheme")
                    .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>("Test-Scheme", ops =>
                    {
                        // TODO: Ensure you seeded user with this id

                        ops.UserId = Guid.NewGuid();
                        ops.ApiScope = AuthorizationScope.RestaurantApi;
                    });
                
                services.AddScoped<IEventBus, FakeEventBus>();
                
                var provider = services.BuildServiceProvider();
    
                using var scope = provider.CreateScope();
                var scopeServices = scope.ServiceProvider;
                
                var dbContext = scopeServices.GetRequiredService<RestaurantContext>();

                dbContext.Database.EnsureDeleted();
                dbContext.Database.Migrate();
            });
        }
    }
}