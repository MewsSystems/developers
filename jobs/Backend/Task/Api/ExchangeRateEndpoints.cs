using ExchangeRateUpdater.Api;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater.Api;

public static class ExchangeRateEndpoints
{
    public static void MapExchangeRateEndpoints(this WebApplication app)
    {
        var api = app.MapGroup("/api/exchange-rates")
            .WithTags("Exchange Rates")
            .WithOpenApi();

        api.MapGet("/", GetExchangeRates)
            .WithName("GetExchangeRates")
            .WithSummary("Get exchange rates for specified currencies")
            .WithDescription("Fetches current exchange rates from Czech National Bank for the specified currency codes. Results are cached for 60 minutes.")
            .Produces<List<ExchangeRateResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        api.MapPost("/", PostExchangeRates)
            .WithName("PostExchangeRates")
            .WithSummary("Get exchange rates for specified currencies (POST)")
            .WithDescription("Fetches current exchange rates from Czech National Bank for the specified currency codes. Use this endpoint for large lists of currencies.")
            .Produces<List<ExchangeRateResponse>>(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)
            .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        api.MapGet("/supported", GetSupportedCurrencies)
            .WithName("GetSupportedCurrencies")
            .WithSummary("Get list of supported currency codes")
            .WithDescription("Returns a list of commonly supported currency codes that can be fetched from Czech National Bank")
            .Produces(StatusCodes.Status200OK);

        app.MapGet("/health", HealthCheck)
            .WithName("HealthCheck")
            .WithTags("Health")
            .WithSummary("Health check endpoint")
            .Produces(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetExchangeRates(
        [FromQuery] string? currencies,
        [FromServices] ExchangeRateProvider provider,
        [FromServices] ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(currencies))
            {
                return Results.BadRequest(new ErrorResponse
                {
                    Error = "Currency codes are required",
                    Details = "Provide currency codes as a comma-separated query parameter (e.g., ?currencies=USD,EUR,GBP)"
                });
            }

            var currencyCodes = currencies.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (currencyCodes.Length == 0)
            {
                return Results.BadRequest(new ErrorResponse
                {
                    Error = "At least one currency code is required"
                });
            }

            var currencyList = currencyCodes.Select(code => new Currency(code)).ToList();

            logger.LogInformation("Fetching exchange rates for currencies: {Currencies}", string.Join(", ", currencyCodes));

            var rates = await provider.GetExchangeRatesAsync(currencyList, cancellationToken);
            var ratesList = rates.ToList();

            var response = ratesList.Select(r => new ExchangeRateResponse
            {
                SourceCurrency = r.SourceCurrency.Code,
                TargetCurrency = r.TargetCurrency.Code,
                Rate = r.Value
            }).ToList();

            logger.LogInformation("Successfully retrieved {Count} exchange rates", response.Count);

            return Results.Ok(response);
        }
        catch (ExchangeRateProviderException ex)
        {
            logger.LogError(ex, "Failed to retrieve exchange rates");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Service Unavailable"
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");
            return Results.Problem(
                detail: "An unexpected error occurred while fetching exchange rates",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }

    private static async Task<IResult> PostExchangeRates(
        [FromBody] ExchangeRateRequest request,
        [FromServices] ExchangeRateProvider provider,
        [FromServices] ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.CurrencyCodes == null || request.CurrencyCodes.Length == 0)
            {
                return Results.BadRequest(new ErrorResponse
                {
                    Error = "Currency codes are required",
                    Details = "Provide at least one currency code in the request body"
                });
            }

            var currencyList = request.CurrencyCodes.Select(code => new Currency(code)).ToList();

            logger.LogInformation("Fetching exchange rates for currencies: {Currencies}", string.Join(", ", request.CurrencyCodes));

            var rates = await provider.GetExchangeRatesAsync(currencyList, cancellationToken);
            var ratesList = rates.ToList();

            var response = ratesList.Select(r => new ExchangeRateResponse
            {
                SourceCurrency = r.SourceCurrency.Code,
                TargetCurrency = r.TargetCurrency.Code,
                Rate = r.Value
            }).ToList();

            logger.LogInformation("Successfully retrieved {Count} exchange rates", response.Count);

            return Results.Ok(response);
        }
        catch (ExchangeRateProviderException ex)
        {
            logger.LogError(ex, "Failed to retrieve exchange rates");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Service Unavailable"
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");
            return Results.Problem(
                detail: "An unexpected error occurred while fetching exchange rates",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }

    private static async Task<IResult> GetSupportedCurrencies(
        [FromServices] ExchangeRateProvider provider,
        [FromServices] ILogger<Program> logger,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Fetching list of supported currencies");

            var supportedCurrencies = await provider.GetSupportedCurrenciesAsync(cancellationToken);

            return Results.Ok(new
            {
                BaseCurrency = "CZK",
                SupportedCurrencies = supportedCurrencies.ToArray(),
                Count = supportedCurrencies.Count(),
                Note = "This list is dynamically fetched from Czech National Bank and includes all currently available currencies."
            });
        }
        catch (ExchangeRateProviderException ex)
        {
            logger.LogError(ex, "Failed to retrieve supported currencies");
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status503ServiceUnavailable,
                title: "Service Unavailable"
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error occurred");
            return Results.Problem(
                detail: "An unexpected error occurred while fetching supported currencies",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal Server Error"
            );
        }
    }

    private static IResult HealthCheck()
    {
        return Results.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "Exchange Rate API"
        });
    }
}
