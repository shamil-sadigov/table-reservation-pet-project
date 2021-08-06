using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.ExecutionContexts;
using Restaurants.Application;
using Restaurants.Application.CommandContract;
using Restaurants.Application.Contracts;
using Restaurants.Application.Pipelines;
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
            
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(IdempotencyPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkPipeline<,>));
            
            return services;
        }
    }
}