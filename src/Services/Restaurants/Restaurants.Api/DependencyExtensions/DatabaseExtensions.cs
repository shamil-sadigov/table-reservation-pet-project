using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reservations.Infrastructure;
using Restaurants.Api.Options;
using Restaurants.Application;
using Restaurants.Infrastructure.Contexts;
using Restaurants.Infrastructure.Repositories;

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
                
                if (environment.IsDevelopment())
                    dbOptions.EnableSensitiveDataLogging();

                dbOptions.EnableDetailedErrors();
                dbOptions.UseLoggerFactory(loggerFactory);
                
                dbOptions.UseSqlServer(connectionString.Value.Default, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure();
                    sqlOptions.MigrationsAssembly(typeof(RestaurantContext).Assembly.FullName);
                });
            });
            
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>((provider) =>
            {
                var connectionStringOptions = provider.GetRequiredService<IOptions<ConnectionStringOptions>>();

                return new SqlConnectionFactory(connectionStringOptions.Value.Default);
            });
            
            services.AddScoped<IRestaurantQueryRepository, RestaurantQueryRepository>();
            
            services.AddScoped<IResilientTransaction, ResilientTransaction>();
            
            
            return services;
        }
    }
}