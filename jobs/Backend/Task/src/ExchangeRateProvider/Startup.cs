using ExchangeRateUpdater.Api.Middlewares;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace ExchangeRateUpdater;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddRouting()
            .AddDistributedMemoryCache()
            .AddFastEndpoints()
            .SwaggerDocument()
            .ConfigureClients()
            .ConfigureOptions(Configuration)
            .AddAutoMapper(typeof(Program))
            .ConfigureHandlers()
            .ConfigureServices()
            .ConfigureFeatureFlags()
            .ConfigureHostedServices()
            .ConfigureHealthChecks();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app
            .UseMiddleware<ExceptionMiddleware>()
            .UseRouting()
            .UseHttpsRedirection()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapFastEndpoints(c =>
                {
                    c.Endpoints.RoutePrefix = "api";
                });
                endpoints.MapCustomHealthChecks();
            });

        if (env.IsDevelopment())
        {
            app.UseSwaggerGen();
        }
    }
}