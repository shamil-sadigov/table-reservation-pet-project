#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Api.Auth;
using Restaurants.Api.Options;

#endregion

namespace Restaurants.Api.DependencyExtensions
{
    public static partial class ServiceExtensions
    {
        public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services,
            IConfiguration configuration)
        {
            var identityServiceOptions = configuration
                .GetSection("IdentityService")
                .Get<IdentityServiceOptions>()
                .EnsureValid();


            services.AddAuthentication("Bearer")
                .AddJwtBearer(ops =>
                {
                    ops.Authority = identityServiceOptions.Url;
                    ops.RequireHttpsMetadata = false;
                    ops.Audience = "restaurants";
                });

            return services;
        }

        public static IServiceCollection AddScopeAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(ops =>
            {
                ops.AddPolicy("restaurant-api-policy",
                    policy => policy.RequireClaim("scope", AuthorizationScope.RestaurantApi));
            });

            return services;
        }
    }
}