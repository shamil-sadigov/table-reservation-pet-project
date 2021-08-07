using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Restaurants.Api.DependencyExtensions;
using Restaurants.Application;

namespace Restaurants.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors()
                .AddValidatorsFromAssemblyContaining<Command>()
                .AddHttpContextAccessor()
                .AddDefaultAuthentication(Configuration)
                .AddScopeAuthorization()
                .AddDatabase()
                .AddApplicationServices()
                .AddIntegrationEventBus()
                .AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                // TODO: Add authentication here
                // when Identity Service will be ready
                
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Restaurants.Api", Version = "v1"
                });
            });
            
            // TODO: Add health checks
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Restaurants.Api v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}