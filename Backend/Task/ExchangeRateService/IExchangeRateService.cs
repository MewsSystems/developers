using System.Collections.Generic;

namespace ExchangeRateService
{
    public interface IExchangeRateService
    {
        IEnumerable<CurrencyData> GetExchangeRateData(IEnumerable<string> currencyCodes);
    }
}
