using System.Collections.Generic;
using ExchangeRateUpdater.model;

namespace ExchangeRateUpdater.services;

public interface IExchangeRateExporter
{
    void ExportExchangeRates(IEnumerable<ExchangeRate> exchangeRates);
}