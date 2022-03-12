using System.Collections.Generic;

namespace ExchangeRateProvider
{
    public interface IExchangeRateFileHandler
    {
        bool IsCachedFileUpToDate();
        IDictionary<CurrencyCode, ExchangeRate> Read();
        void Write(IDictionary<CurrencyCode, ExchangeRate> exchangeRates);
    }
}