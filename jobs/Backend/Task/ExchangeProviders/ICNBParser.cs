using ExchangeRateUpdater.Models;
using FluentResults;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeProviders
{
    public interface ICNBParser
    {
        Result<ExchangeRates> Parse(string data, IEnumerable<Currency> currencies);
    }
}
