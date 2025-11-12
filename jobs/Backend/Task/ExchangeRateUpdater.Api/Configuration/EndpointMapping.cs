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
            .WithDescription(
                "Note for the test reviewer: There are limitations in .NET 8 Minimal APIs and Swagger. Certain customization and inference isn't supported around some types of metadata (e.g. description, examples, responses etc.)")
            .MapExchangeRateEndpoints();
    }
}