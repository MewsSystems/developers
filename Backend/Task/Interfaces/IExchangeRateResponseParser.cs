using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateResponseParser
    {
        IEnumerable<ExchangeRate> ParseResponse(string response, Currency targetCurrency);
    }
}
