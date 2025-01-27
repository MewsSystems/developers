using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.RateSources;

public interface IRateSource
{
    string SourceName { get; }
    ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate);
}