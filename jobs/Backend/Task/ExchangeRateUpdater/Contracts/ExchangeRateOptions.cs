using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Contracts;

public class ExchangeRateOptions
{
    public IReadOnlyCollection<Currency> RequestedCurrencies { get; init; } = Array.Empty<Currency>();
}