#region

using EventBus.Abstractions;
using EventBus.RabbitMq;
using EventBus.RabbitMq.Abstractions;
using EventBus.RabbitMq.Database;
using EventBus.RabbitMq.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.UseCases.Restaurants.RequestTableReservation.IntegrationEvent;

#endregion

namespace Restaurants.Api.DependencyExtensions
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddIntegrationEventBus(this IServiceCollection services)
        {
            services.AddScoped<IIntegrationEventRepository, IntegrationEventRepository>();

            services.AddScoped<IIntegrationEventsPublisher, IntegrationEventsPublisher>();

            var integrationEventsAssembly = typeof(TableReservationIsRequestedIntegrationEvent).Assembly;

            services.AddSingleton(new IntegrationEventTypesAccessor(integrationEventsAssembly));

            services.AddSingleton<IntegrationEventDeserializer>();

            services.AddScoped<IEventBus, RabbitMqEventBus>();

            return services;
        }
    }
}