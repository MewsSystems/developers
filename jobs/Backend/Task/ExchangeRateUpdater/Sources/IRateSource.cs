using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources;

public interface IRateSource
{
    string SourceName { get; }
    ValueTask<IReadOnlyList<ExchangeRate>> GetRatesAsync(DateOnly targetDate);
}