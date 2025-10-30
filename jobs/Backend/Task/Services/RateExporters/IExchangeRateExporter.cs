using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Services.RateExporters;

public interface IExchangeRateExporter
{
    Task ExportExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates);
}