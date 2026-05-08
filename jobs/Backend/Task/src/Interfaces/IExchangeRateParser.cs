using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateParser
{
    List<ExchangeRate> Parse(string data, Currency baseCurrency);
}