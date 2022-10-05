using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Common.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private ILogger _logger;
        private IConfiguration _configuration;
        private IHttpWrapper _httpWrapper;

        private const string CZK_CODE = "CZK";
        private Currency targetCurrency = new Currency(CZK_CODE);

        public ExchangeRateProvider(ILogger<ExchangeRateProvider> logger, IConfiguration configuration, IHttpWrapper httpWrapper)
        {
            _logger = logger;
            _configuration = configuration;
            _httpWrapper = httpWrapper;
        }
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var rates = Enumerable.Empty<ExchangeRate>();
            var calculatedRates = new List<ExchangeRate>();

            try
            {
                var rateData = await GetRates();
                _logger.LogDebug($"Rate data \r\n {rateData}");

                var parsedRates = ParseRates(rateData);

                foreach (ExchangeRateItem cnbData in parsedRates)
                {
                    calculatedRates.Add(new ExchangeRate(
                        new Currency(cnbData.Code),
                        targetCurrency,
                        cnbData.Rate / cnbData.Amount
                        ));
                }

                rates = calculatedRates.Where(rates => currencies.Any(currency => currency.Code.Equals(rates.SourceCurrency.Code)));
            } 
            catch (Exception ex)
            {
                _logger.LogError($"Failed to retrieve and parse exchange rates: {Environment.NewLine}");
                _logger.LogError(ex, ex.Message);
            }

            return rates;
        }

        /// <summary>
        /// Helper method to fetch the rates from the source
        /// TODO: Improve strucutre
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetRates()
        {
            // pull source from config
            // TODO: Null handling for config value
            var rateSource = _configuration.GetValue<string>("ExchangeRateProvider:Source");
            var responseContent = await _httpWrapper.HttpGet(rateSource);

            return responseContent;
        }

        /// <summary>
        /// Helper method to parse rate data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private IEnumerable<ExchangeRateItem> ParseRates(string data)
        {
            var _csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = "|",
                HasHeaderRecord = true
            };

            // ensure input is valid, exception handling should be done by calling code
            if (data == null || string.IsNullOrWhiteSpace(data) || data.Length <= 0)
            {
                throw new ArgumentNullException(nameof(data), "String input is required");
            }

            var textReader = new StringReader(data);
            using var csv = new CsvReader(textReader, _csvConfiguration);
            // skip the first line because it isn't used
            csv.Read();
            return csv.GetRecords<ExchangeRateItem>().ToList();
        }
    }
}
