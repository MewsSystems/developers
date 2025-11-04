using ExchangeRateUpdater.Models;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Services.Builders
{
    public interface IExchangeRateBuilder
    {
        IEnumerable<ExchangeRate> BuildExchangeRates(IEnumerable<Currency> requestedCurrencies, CnbExchangeRateData cnbData);
    }
}
