using ExchangeRateProvider.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace ExchangeRateProvider.Infrastructure.ExternalServices.CZK;

public class CzkExchangeRateMapper(ILogger<CzkExchangeRateMapper> logger) : ICzkExchangeRateMapper
{
    public IList<ExchangeRate> MapToExchangeRates(CzkExchangeRateResponse? response, Currency baseCurrency)
    {
        if (response?.Rates is null)
        {
            logger.LogWarning("Received null or empty response from CNB API");
            return new List<ExchangeRate>();
        }

        var exchangeRates = response.Rates
            .Select(rate => new ExchangeRate(
                baseCurrency,
                new Currency(rate.CurrencyCode),
                rate.Rate / rate.Amount))
            .ToList();

        logger.LogInformation("Mapped {Count} exchange rates from CNB response", exchangeRates.Count);

        return exchangeRates;
    }
}
