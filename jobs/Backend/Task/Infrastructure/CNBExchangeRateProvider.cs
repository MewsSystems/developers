using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Infrastructure
{
    internal sealed class CNBExchangeRateProvider(
        HttpClient httpClient, 
        IExchangeRateParser parser) : IExchangeRateProvider
    {
        private const string RatesSegment = "daily.txt";

        private readonly HttpClient httpClient = httpClient;
        private readonly IExchangeRateParser parser = parser;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {     
            var response = await httpClient.GetAsync(RatesSegment);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            return parser.Parse(content);
        }
    }
}
