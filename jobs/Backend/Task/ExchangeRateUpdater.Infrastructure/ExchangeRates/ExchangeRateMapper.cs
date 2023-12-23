using ExchangeRateUpdater.Infrastructure.ExchangeRates.CnbApi;
using ExchangeRateUpdater.Model.ExchangeRates;

namespace ExchangeRateUpdater.Infrastructure.ExchangeRates;

public class ExchangeRateMapper : IExchangeRateMapper
{
    public IEnumerable<ExchangeRate> MapFromCnbExchangeRatesDailyResponse(CnbExchangeRatesDailyResponse? response)
    {
        if (response?.Rates is null)
        {
            return Enumerable.Empty<ExchangeRate>();
        }
        
        return response.Rates.Select(exchangeRate => new ExchangeRate(
            Currency.Czk,
            new Currency(exchangeRate.CurrencyCode),
            exchangeRate.Rate
        ));
    }
}