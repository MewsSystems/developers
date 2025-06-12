using ExchangeRateUpdater.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    private readonly IExchangeRateDataProvider _exchangeRateDataProvider;

    public ExchangeRateProvider(IExchangeRateDataProvider exchangeRateDataProvider)
    {
        _exchangeRateDataProvider = exchangeRateDataProvider;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
    {
        if (currencies == null || !currencies.Any())
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        // Get normalized rates for the requested currencies
        var currencyRates = await _exchangeRateDataProvider.GetNormalizedRatesAsync(currencies);

        if (currencyRates.Count < 2)
        {
            return Enumerable.Empty<ExchangeRate>();
        }

        var exchangeRates = new List<ExchangeRate>();
        
        // Add ExchangeRate for every combination of source and target (excluding same currency)
        foreach (var source in currencyRates)
        {
            foreach (var target in currencyRates)
            {
                if (source.Key.Equals(target.Key, StringComparison.OrdinalIgnoreCase))
                {
                    continue; // Skip if same currency.
                }

                // Calculate the exchange rate as the ratio of the normalized rates.
                var exchangeRateValue = 
                    Math.Round(source.Value.Rate / target.Value.Rate, 3, MidpointRounding.AwayFromZero);
                exchangeRates.Add(new ExchangeRate(source.Value.Currency, target.Value.Currency, exchangeRateValue));
            }
        }

        return exchangeRates;
    }
}
