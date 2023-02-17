using ExchangeRateUpdater.Abstractions;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Data;

public sealed class ExchangeRateCache : IExchangeRateCache
{
    public Dictionary<Currency, List<ExchangeRate>> SourceExchangeRates { get; } = new();
    public Dictionary<Currency, List<ExchangeRate>> TargetExchangeRates { get; } = new();
    public int Count { get; set; } = 0;

    public void Add(ExchangeRate exchangeRate)
    {
        AddToDictionary(SourceExchangeRates, exchangeRate.SourceCurrency, exchangeRate);
        AddToDictionary(TargetExchangeRates, exchangeRate.TargetCurrency, exchangeRate);
        Count++;
    }

    private static void AddToDictionary(IDictionary<Currency, List<ExchangeRate>> dictionary, Currency currency, ExchangeRate exchangeRate)
    {
        if (dictionary.TryGetValue(currency, out List<ExchangeRate> list))
        {
            list.Add(exchangeRate);
        }
        else
        {
            dictionary.Add(currency, new List<ExchangeRate>() { exchangeRate });
        }
    }

    public void Clear()
    {
        SourceExchangeRates.Clear();
        TargetExchangeRates.Clear();
    }
}
