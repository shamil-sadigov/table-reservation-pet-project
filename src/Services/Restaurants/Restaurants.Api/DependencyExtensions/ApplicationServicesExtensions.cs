using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.ExecutionContexts;
using Restaurants.Application;
using Restaurants.Application.Behaviors;
using Restaurants.Application.CommandContract;
using Restaurants.Application.Contracts;
using Restaurants.Domain.Restaurants.Contracts;
using Restaurants.Infrastructure;

namespace Restaurants.Api.DependencyExtensions
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IExecutionContext, WebExecutionContext>();
            services.AddScoped<IIdentityProvider, IdentityProvider>();
            services.AddTransient<IRestaurantChecker, RestaurantChecker>();
            
            services.AddMediatR(typeof(Command));
            
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>));
            
            return services;
        }
    }
}