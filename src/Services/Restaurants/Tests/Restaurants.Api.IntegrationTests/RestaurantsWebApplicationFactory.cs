#region

using System;
using System.IO;
using EventBus.RabbitMq.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.IntegrationTests.DataSeeders;
using Restaurants.Api.IntegrationTests.Helpers;
using Restaurants.Infrastructure.Contexts;

#endregion

namespace Restaurants.Api.IntegrationTests
{
    public class RestaurantsWebApplicationFactory : WebApplicationFactory<Startup>
    {
        public Action<IServiceCollection>? ConfigureServices;
        
        protected IDataSeeder? DataSeeder;
        
        public RestaurantsWebApplicationFactory WithPredefinedData(IDataSeeder dataSeeder)
        {
            DataSeeder = dataSeeder;
            return this;
        }
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
                ConfigureServices?.Invoke(services);
                
                using var provider = services.BuildServiceProvider();
                using var scope = provider.CreateScope();

                var servicesProvider = scope.ServiceProvider;

                servicesProvider.RecreateDatabase<RestaurantContext>(DataSeeder);
                servicesProvider.MigrateDatabase<IntegrationEventContext>();
            });
        }
    }
}