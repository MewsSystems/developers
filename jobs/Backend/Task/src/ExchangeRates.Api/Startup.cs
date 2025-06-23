using FastEndpoints.Swagger;

namespace ExchangeRates.Api;

public class Startup
{
    private IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddRouting()
            .AddEndpointsApiExplorer()
            .AddFastEndpoints()
            .SwaggerDocument()
            .AddMediator()
            .AddDistributedMemoryCache()
            .AddExchageRateApiOptions(Configuration)
            .AddExchangeRatesApiProviders()
            .AddExchangeRatesApiHttpClients();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapFastEndpoints(config =>
            {
                config.Endpoints.RoutePrefix = "api";
            });
        });

        if (env.IsDevelopment())
        {
            app.UseSwaggerGen();
        }
    }
}