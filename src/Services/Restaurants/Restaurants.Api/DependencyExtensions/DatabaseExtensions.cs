#region

using EventBus.RabbitMq.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Restaurants.Api.Options;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Infrastructure;
using Restaurants.Infrastructure.Contexts;
using Restaurants.Infrastructure.Repositories;

#endregion

namespace Restaurants.Api.DependencyExtensions
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddOptions<ConnectionStringOptions>()
                .BindConfiguration("ConnectionStrings")
                .ValidateDataAnnotations();

            services.AddDbContext<RestaurantContext>((provider, dbOptions) =>
            {
                var connectionString = provider.GetRequiredService<IOptions<ConnectionStringOptions>>();
                var environment = provider.GetRequiredService<IHostEnvironment>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                if (environment.IsDevelopment() || environment.IsTesting())
                    dbOptions.EnableSensitiveDataLogging();

                dbOptions.EnableDetailedErrors();
                dbOptions.UseLoggerFactory(loggerFactory);

                dbOptions.UseSqlServer(connectionString.Value.Default, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsAssembly(typeof(RestaurantContext).Assembly.FullName);
                });
            });

            services.AddDbContext<IntegrationEventContext>((provider, dbOptions) =>
            {
                var restaurantContext = provider.GetRequiredService<RestaurantContext>();

                var environment = provider.GetRequiredService<IHostEnvironment>();
                var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

                if (environment.IsDevelopment() || environment.IsTesting())
                    dbOptions.EnableSensitiveDataLogging();

                dbOptions.EnableDetailedErrors();
                dbOptions.UseLoggerFactory(loggerFactory);

                dbOptions.UseSqlServer(
                    restaurantContext.Database.GetDbConnection(),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure();
                        sqlOptions.MigrationsAssembly(typeof(IntegrationEventContext).Assembly.FullName);
                    });
            });


            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>(provider =>
            {
                var connectionStringOptions = provider.GetRequiredService<IOptions<ConnectionStringOptions>>();

                return new SqlConnectionFactory(connectionStringOptions.Value.Default);
            });

            services.AddScoped<IRestaurantQueryRepository, RestaurantQueryRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            services.AddScoped<ICommandRepository, CommandRepository>();

            services.AddScoped<IResilientTransaction, ResilientTransaction>();
            services.AddScoped<IDbTransactionProvider, RestaurantTransactionProvider>();

            return services;
        }
    }
}