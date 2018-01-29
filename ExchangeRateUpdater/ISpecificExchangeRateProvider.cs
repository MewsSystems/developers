using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public interface ISpecificExchangeRateProvider
    {
        string ProviderName { get; }
        string BaseURL { get; }
        Currency BaseCurrency { get; }

        IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
