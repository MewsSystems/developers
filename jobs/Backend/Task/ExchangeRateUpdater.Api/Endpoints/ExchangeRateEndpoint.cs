namespace ExchangeRate.Api.Endpoints;

public static class ExchangeRateEndpoint
{
    public static void MapExchangeRateEndpoints(this RouteGroupBuilder group)
    {
        group.MapGet("/cnb", Handle);
    }

    private static Task<IResult> Handle()
    {
        return Task.FromResult(Results.Ok());
    }
}