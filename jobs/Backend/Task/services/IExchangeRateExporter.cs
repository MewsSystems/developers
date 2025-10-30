using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.model;

namespace ExchangeRateUpdater.services;

public interface IExchangeRateExporter
{
    Task ExportExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates);
}