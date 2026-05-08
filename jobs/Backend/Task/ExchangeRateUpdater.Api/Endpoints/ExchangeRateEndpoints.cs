using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Validation;

namespace ExchangeRateUpdater.Api.Endpoints;

public static class ExchangeRateEndpoints
{
    public static void MapExchangeRatesEndpoints(this WebApplication app)
    {
        app.MapGet("/exchange-rates", async (
                string[] currencies, 
                IExchangeRateProvider exchangeRateProvider,
                CancellationToken cancellationToken) =>
            {
                var validator = new ExchangeRatesRequestValidator();
                var validationResult = await validator.ValidateAsync(currencies, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
                }

                var result = await exchangeRateProvider.GetExchangeRates(currencies, cancellationToken);
                
                return Results.Ok(result);
            })
            .WithName("GetExchangeRates")
            .WithDescription("Gets exchange rates for provided currencies to CZK from Czech National Bank")
            .WithTags("Exchange Rates");
    }
}