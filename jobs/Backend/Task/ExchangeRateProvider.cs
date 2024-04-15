using ExchangeRateUpdater.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellatio);
    }

    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly Currency _targetCurrency = new Currency(ConfigurationManager.AppSettings["targetCurrency"]);
        private readonly string _source = ConfigurationManager.AppSettings["sourceUrl"];
        private readonly string _apiSource = ConfigurationManager.AppSettings["apiUrl"];
        private IEnumerable<ExchangeRate> _scrappedRates = new List<ExchangeRate>();
        private readonly IHttpClientFactory _httpClientFactory;

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger = null, IHttpClientFactory httpClientFactory = null)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, CancellationToken cancellation)
        {
            if (_scrappedRates == null || !_scrappedRates.Any())
            {
                await GetDataFromApi(cancellation);
            }

            //backup
            if (_scrappedRates == null | !_scrappedRates.Any())
            {
                await GetDataFromWeb(cancellation);
            }

            return _scrappedRates?.Where(w => currencies.Contains(w.SourceCurrency));
        }

        private async Task GetDataFromApi(CancellationToken cancellation)
        {
            var cron = DateTime.UtcNow;
            string json = await LoadApi(cancellation);
            var result = new List<ExchangeRate>();

            if (json != null)
            {
                var deserialized = JsonConvert.DeserializeObject<ApiResponse>(json);

                if (deserialized != null)
                {
                    foreach (var rate in deserialized.rates)
                    {
                        result.Add(new ExchangeRate(new Currency(rate.currencyCode), _targetCurrency, Convert.ToDecimal(rate.rate)));
                    }
                }
            }

            _logger?.LogInformation($"Parsing data from API took {(DateTime.UtcNow - cron).TotalMilliseconds}ms for {result.Count} exchange rates");
            _scrappedRates = result;
        }

        public virtual async Task<string> LoadApi(CancellationToken cancellation)
        {
            var cron = DateTime.UtcNow;
            var client = _httpClientFactory.CreateClient();
            var reqMsg = new HttpRequestMessage(HttpMethod.Get, _apiSource);
            var response = await client.SendAsync(reqMsg);

            if (response != null)
            {
                var content = await response.Content.ReadAsStringAsync(cancellation);

                if (!string.IsNullOrEmpty(content))
                {
                    _logger?.LogInformation($"Retrieving data from API took {(DateTime.UtcNow - cron).TotalMilliseconds}ms from {_source}");
                    return content;
                }
            }

            _logger?.LogWarning("Couldn't load data from API");
            return null;
        }

        private async Task GetDataFromWeb(CancellationToken cancellation)
        {
            var cron = DateTime.UtcNow;
            _logger?.LogInformation($"Scraping data");
            var result = new List<ExchangeRate>();
            var rawDocument = await LoadWeb(cancellation);

            if (rawDocument == null)
            {
                _logger?.LogWarning("No data retrieved");
                return;
            }

            var rawData = rawDocument.QuerySelectorAll("table.currency-table").QuerySelectorAll("tbody").QuerySelectorAll("tr");

            foreach (var row in rawData)
            {
                var cells = row.QuerySelectorAll("td");

                if (cells.Count != 5) continue;

                result.Add(new ExchangeRate(new Currency(cells[3].InnerText), _targetCurrency, decimal.Parse(cells[4].InnerText)));
            }
            _logger?.LogInformation($"Scraping data took {(DateTime.UtcNow - cron).TotalMilliseconds}ms for {result.Count} exchange rates");
            _scrappedRates = result;
        }

        public virtual async Task<HtmlNode> LoadWeb(CancellationToken cancellation)
        {
            var web = new HtmlWeb();
            var cron = DateTime.UtcNow;
            var document = await web.LoadFromWebAsync(_source, cancellation);
            _logger?.LogInformation($"Retrieving data took {(DateTime.UtcNow - cron).TotalMilliseconds}ms from {_source}");
            if (document == null)
            {
                _logger?.LogWarning("Couldn't load data from website");
                return null;
            }

            return document.DocumentNode;
        }
    }
}
