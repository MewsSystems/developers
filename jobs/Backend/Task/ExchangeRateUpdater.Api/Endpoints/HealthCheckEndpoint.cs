namespace ExchangeRate.Api.Endpoints;

public static class HealthCheckEndpoint
{
    public static void MapHealthCheckEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/api", () => Results.Ok())
            .WithOpenApi();
    }
}