using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Options;
using System.Xml.Linq;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;

        private const string DefaultExchangeCode = "CZK";

        public ExchangeRateProvider(IOptions<CNBConfigurationOptions> options, HttpClient httpClient)
        {
            _url = options.Value.DataURL;
            _httpClient = httpClient;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var response = await _httpClient.GetAsync(_url);
            var responseString = await response.Content.ReadAsStringAsync();
            var doc = XDocument.Parse(responseString);

            var currencyCodes = currencies.Select(c => c.Code).ToHashSet();

            var rates = doc.Descendants("radek")
                       .Where(attr => currencyCodes.Contains(attr.Attribute("kod")?.Value))
                       .Select(attr => new ExchangeRate(
                               new(DefaultExchangeCode),
                               new(attr.GetExchangeCode()),
                               attr.GetExchangeRate() / attr.GetExchangeAmount())
                       );

            return rates;
        }
    }
}
