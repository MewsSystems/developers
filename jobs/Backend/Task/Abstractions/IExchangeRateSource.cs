using ExchangeRateUpdater.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Abstractions;

public interface IExchangeRateSource
{
    Task LoadAsync();

    IEnumerable<ExchangeRate> GetExchangeRates();
}