using ExchangeRateProvider.Api.Constants;
using ExchangeRateProvider.Api.Dto;
using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Domain.Entities;

namespace ExchangeRateProvider.Api.Endpoints;

/// <summary>
/// Contains extension methods for mapping exchange rate related API endpoints.
/// </summary>
public static class ExchangeRateEndpoints
{
    /// <summary>
    /// Maps the GET /exchange-rates endpoint to the application's route builder.
    /// The endpoint requires authentication, applies rate limiting, and returns the latest exchange rates
    /// for comma-separated currency codes provided via query parameter.
    /// </summary>
    /// <param name="app">The endpoint route builder to add the exchange rate endpoints to.</param>
    /// <returns>The endpoint route builder for method chaining.</returns>
    public static IEndpointRouteBuilder MapExchangeRatesEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/exchange-rates",
                async (
                    IExchangeRateProviderService exchangeRateProvider,
                    ILogger<Program> logger,
                    string? currencies,
                    CancellationToken cancellationToken) =>
                {
                    if (string.IsNullOrWhiteSpace(currencies))
                    {
                        logger.LogWarning("Exchange rates request rejected: currencies parameter is missing or empty");
                        return Results.BadRequest("Query string parameter 'currencies' is required. Example: ?currencies=USD,EUR");
                    }

                    var currencyList = currencies
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .Select(code => new Currency(code));

                    var exchangeRates = await exchangeRateProvider
                        .GetLatestAsync(currencyList, cancellationToken);

                    var response = exchangeRates.Select(r =>
                        new ExchangeRateDto(r.SourceCurrency.Code, r.TargetCurrency.Code, r.Value));

                    return Results.Ok(response);
                })
            .WithName("GetLatestExchangeRates")
            .WithSummary("Get latest exchange rates")
            .WithDescription("Retrieves the latest exchange rates for the specified currencies")
            .Produces<IEnumerable<ExchangeRateDto>>()
            .Produces(StatusCodes.Status401Unauthorized)
            .Produces<string>(StatusCodes.Status400BadRequest)
            .Produces<string>(StatusCodes.Status500InternalServerError)
            .RequireAuthorization()
            .RequireRateLimiting(RateLimitPolicies.ExchangeRates);

        return app;
    }
}