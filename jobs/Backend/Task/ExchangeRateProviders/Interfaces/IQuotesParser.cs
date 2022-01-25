using ExchangeRateUpdater.CoreClasses;
using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateProviders.Interfaces
{
    public interface IQuotesParser
    {
        IDictionary<Currency, ExchangeRate> ParseQuotes(Currency targetCurrency, string quotes);
    }
}
