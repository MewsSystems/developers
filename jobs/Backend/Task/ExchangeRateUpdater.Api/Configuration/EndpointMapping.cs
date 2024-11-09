using ExchangeRate.Api.Endpoints;

namespace ExchangeRate.Api.Configuration;

public static class EndpointMapping
{
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGroup("healthcheck")
            .WithOpenApi()
            .MapHealthCheckEndpoints();

        app.MapGroup("exrates")
            .WithOpenApi()
            .MapExchangeRateEndpoints();
    }
}