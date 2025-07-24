using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Xml;
using System.Xml.Linq;

namespace ExchangeRateUpdater.Services
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly HttpClient _httpClient;
        private readonly string _url;

        private const string DefaultExchangeCode = "CZK";

        public ExchangeRateProvider(IOptions<CNBConfigurationOptions> options,
            HttpClient httpClient,
            ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _logger = logger;

            _url = options.Value.DataURL;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies is null || !currencies.Any())
            {
                _logger.LogError("GetExchangeRates: You must to specify currencies to calculate exchange rates.");
                throw new ArgumentNullException(nameof(currencies));
            }

            try
            {
                var currencyCodes = currencies.Select(c => c.Code).ToHashSet();

                _logger.LogInformation("GetExchangeRates: Getting list of exchange rates from {CNBURL}", _url);
                var response = await _httpClient.GetAsync(_url);
                response.EnsureSuccessStatusCode();

                var responseString = await response.Content.ReadAsStringAsync();
                var doc = XDocument.Parse(responseString);

                _logger.LogInformation("GetExchangeRates: Calculating exchange rates for {CurrencyCodes}", string.Join(",", currencyCodes));
                var rates = doc.Descendants("radek")
                    .Where(attr => currencyCodes.Contains(attr.GetExchangeCode()))
                    .Select(attr => new ExchangeRate(
                            new(DefaultExchangeCode),
                            new(attr.GetExchangeCode()),
                            attr.GetExchangeRate() / attr.GetExchangeAmount())
                    );

                return rates;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("GetExchangeRates: HTTP request failed for URL: {URL}", _url);
                return Enumerable.Empty<ExchangeRate>();
            }
            catch (XmlException ex)
            {
                _logger.LogError("GetExchangeRates: XML parsing failed {XmlErrorMessage}", ex.Message);
                return Enumerable.Empty<ExchangeRate>();
            }
            catch(TaskCanceledException ex)
            {
                _logger.LogError("GetExchangeRates: Request time out {TimeoutErrorMessage}", ex.Message);
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}
