using ExchangeRateUpdater.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRatePrinter
{
    Task PrintRates(IEnumerable<Currency> currencies);
}