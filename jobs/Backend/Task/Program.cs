using ExchangeRateUpdater.Api;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRateUpdater;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new()
            {
                Title = "Exchange Rate API",
                Version = "v1",
                Description = "API for fetching exchange rates from Czech National Bank",
                Contact = new()
                {
                    Name = "Exchange Rate Updater",
                    Url = new Uri("https://github.com/MewsSystems/developers")
                }
            });
        });

        // Add Exchange Rate Provider services
        builder.Services.AddExchangeRateProvider(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exchange Rate API v1");
                c.RoutePrefix = string.Empty; // Serve Swagger UI at root
            });
        }

        app.UseHttpsRedirection();

        // API Endpoints
        ConfigureEndpoints(app);

        app.Run();
    }

    private static void ConfigureEndpoints(WebApplication app)
    {
        var api = app.MapGroup("/api/exchange-rates")
            .WithTags("Exchange Rates")
            .WithOpenApi();

        // GET /api/exchange-rates?currencies=USD,EUR,GBP
        api.MapGet("/", async (
            [FromQuery] string currencies,
            [FromServices] ExchangeRateProvider provider,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
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
        })
        .WithName("GetExchangeRates")
        .WithSummary("Get exchange rates for specified currencies")
        .WithDescription("Fetches current exchange rates from Czech National Bank for the specified currency codes. Results are cached for 60 minutes.")
        .Produces<List<ExchangeRateResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // POST /api/exchange-rates
        api.MapPost("/", async (
            [FromBody] ExchangeRateRequest request,
            [FromServices] ExchangeRateProvider provider,
            [FromServices] ILogger<Program> logger,
            CancellationToken cancellationToken) =>
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
        })
        .WithName("PostExchangeRates")
        .WithSummary("Get exchange rates for specified currencies (POST)")
        .WithDescription("Fetches current exchange rates from Czech National Bank for the specified currency codes. Use this endpoint for large lists of currencies.")
        .Produces<List<ExchangeRateResponse>>(StatusCodes.Status200OK)
        .Produces<ErrorResponse>(StatusCodes.Status400BadRequest)
        .Produces<ProblemDetails>(StatusCodes.Status503ServiceUnavailable)
        .Produces<ProblemDetails>(StatusCodes.Status500InternalServerError);

        // GET /api/exchange-rates/supported
        api.MapGet("/supported", (
            [FromServices] ILogger<Program> logger) =>
        {
            logger.LogInformation("Fetching list of supported currencies");

            // Common currencies supported by CNB (this is a subset, CNB supports many more)
            var supportedCurrencies = new[]
            {
                "AUD", "BRL", "BGN", "CAD", "CNY", "DKK", "EUR", "PHP", "HKD", "HUF",
                "INR", "IDR", "ISK", "ILS", "JPY", "ZAR", "KRW", "MXN", "NOK", "NZD",
                "PLN", "RON", "RUB", "SGD", "SEK", "CHF", "THB", "TRY", "USD", "GBP"
            };

            return Results.Ok(new
            {
                BaseCurrency = "CZK",
                SupportedCurrencies = supportedCurrencies.OrderBy(c => c).ToArray(),
                Note = "This list includes commonly available currencies. CNB may support additional currencies."
            });
        })
        .WithName("GetSupportedCurrencies")
        .WithSummary("Get list of supported currency codes")
        .WithDescription("Returns a list of commonly supported currency codes that can be fetched from Czech National Bank")
        .Produces(StatusCodes.Status200OK);

        // Health check endpoint
        app.MapGet("/health", () => Results.Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "Exchange Rate API"
        }))
        .WithName("HealthCheck")
        .WithTags("Health")
        .WithSummary("Health check endpoint")
        .Produces(StatusCodes.Status200OK);
    }
}
