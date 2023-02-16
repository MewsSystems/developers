using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Data;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRatePrinter
{
    Task PrintRates(IEnumerable<Currency> currencies);
}