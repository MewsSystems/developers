using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ExchangeRateUpdater.Helpers;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater.Providers;

public abstract class ExchangeRateProviderBase : IExchangeRateProvider
{
    private readonly IExchangeRateService _exchangeRateService;
    protected abstract Currency TargetCurrency { get; }
    
    protected ExchangeRateProviderBase(IExchangeRateService exchangeRateService)
    {
        ArgumentsHelper.ThrowIfNull("exchangeRateService", exchangeRateService);

        _exchangeRateService = exchangeRateService;
    }

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    /// <param name="currencies">The list of currencies for which to get the exchange rate.</param>
    public async Task<IReadOnlyCollection<ExchangeRate>> GetExchangeRatesAsync(IReadOnlyCollection<Currency> currencies)
    {
        var resultCurrencies = await _exchangeRateService.GetCurrenciesAsync();
        var resultRates = new List<ExchangeRate>();

        if (CollectionHelper.IsEmpty(currencies) || CollectionHelper.IsEmpty(resultCurrencies))
        {
            return resultRates;
        }

        foreach (var currency in currencies)
        {
            var currentExchangeRate = resultCurrencies.FirstOrDefault(x => x.Code == currency.Code);
            if (currentExchangeRate == null)
            {
                continue;
            }

            var rate = new ExchangeRate(currency, TargetCurrency, currentExchangeRate.Rate / currentExchangeRate.Amount);

            resultRates.Add(rate);
        }

        return resultRates;
    }
}