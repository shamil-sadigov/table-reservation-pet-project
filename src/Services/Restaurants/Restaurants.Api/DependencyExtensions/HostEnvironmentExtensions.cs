using System;
using Microsoft.Extensions.Hosting;

namespace Restaurants.Api.DependencyExtensions
{
    public static class HostEnvironmentExtensions
    {
        // 'Testing' environment is set when integration tests are running
        public static bool IsTesting(this IHostEnvironment environment) 
            => environment.EnvironmentName.Equals("Testing", StringComparison.OrdinalIgnoreCase);
    }
}