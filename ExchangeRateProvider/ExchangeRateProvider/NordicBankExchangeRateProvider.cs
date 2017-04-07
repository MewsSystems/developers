using System.Collections.Generic;
using ExchangeRateProvider.Model;

namespace ExchangeRateProvider
{
    /// <summary>
    /// NordicBankExchangeRateProvider
    /// </summary>
    public class NordicBankExchangeRateProvider : AbstractExchangeRateProvider
    {
        public NordicBankExchangeRateProvider(string apiUrl) : base(apiUrl)
        {
        }

        public override IEnumerable<ExchangeRateDto> GetExchangeRates(IEnumerable<CurrencyDto> currencies)
        {
            return Currency.GetExchangeRatesAsync().Result;
        }
    }
}