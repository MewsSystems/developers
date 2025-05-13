using ExchangeRateUpdater.Entities;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Providers;

public class ExchangeRateProvider(IExchangeRateRepository exchangeRateRepository) : IExchangeRateProvider
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        HashSet<string> currencyCodes = [.. currencies.Select(x => x.Code)];

        IEnumerable<ExchangeRate> exchangeRates = await exchangeRateRepository.GetCzkExchangeRatesAsync();

        IEnumerable<ExchangeRate> filteredRates = exchangeRates.Where(r => currencyCodes.Contains(r.SourceCurrency.Code));

        return filteredRates;
    }
}
