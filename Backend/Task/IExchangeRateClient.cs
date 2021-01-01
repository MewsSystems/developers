using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateClient
    {
        IAsyncEnumerable<(int amout, string code, decimal rate)> GetExchanges(IEnumerable<Currency> currencies);
    }
}
