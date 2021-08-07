using System;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.IntegrationTests.Auth;
using Restaurants.Api.IntegrationTests.DataSeeders;

namespace Restaurants.Api.IntegrationTests.Helpers
{
    public static class ServiceProviderExtensions
    {
        public static void RecreateDatabase<TDbContext>(
            this IServiceProvider serviceProvider, 
            IDataSeeder? dataSeeder = null)
            where TDbContext: DbContext
        {
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();
            dataSeeder?.Seed(dbContext);
        }
        
        public static void MigrateDatabase<TDbContext>(
            this IServiceProvider serviceProvider, 
            IDataSeeder? dataSeeder = null)
            where TDbContext: DbContext
        {
            var dbContext = serviceProvider.GetRequiredService<TDbContext>();

            dbContext.Database.Migrate();
            dataSeeder?.Seed(dbContext);
        }
        
        public static void AddTestAuthentication(this IServiceCollection services, Claim[] claims)
        {
            services.AddAuthentication("Test-Scheme")
                .AddScheme<TestAuthenticationSchemeOptions, TestAuthenticationHandler>("Test-Scheme", ops =>
                {
                    ops.Claims = claims;
                });
        }
    }
}