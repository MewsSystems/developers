using ExchangeRateUpdater.Data;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRateCache
{
    Dictionary<Currency, List<ExchangeRate>> SourceExchangeRates { get; }
    Dictionary<Currency, List<ExchangeRate>> TargetExchangeRates { get; }

    void Add(ExchangeRate exchangeRate);
    void Clear();
}