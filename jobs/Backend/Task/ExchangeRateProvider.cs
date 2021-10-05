using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public abstract class ExchangeRateProvider
    {
        protected string SourceUrlBase { get; private set; }
                    
        public ExchangeRateProvider(string sourceUrlBase)
        {
            SourceUrlBase = sourceUrlBase;
        }

        public abstract IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies);

        protected virtual async Task<string> GetStringDataFromSource(string sourceUrl)
        {
            using (var httpClient = new HttpClient())
            {
                var responseStringData = await httpClient.GetStringAsync(sourceUrl);
                return responseStringData;
            }
        }
    }
}
