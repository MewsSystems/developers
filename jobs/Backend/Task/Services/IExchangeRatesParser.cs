using System.Collections.Generic;
using ExchangeRateProvider.Models;

namespace ExchangeRateProvider.Services;

public interface IExchangeRatesParser
{
    IEnumerable<ExchangeRate> ExtractCurrencyExchangeRates(string targetCurrencyCode, string source);
}