using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public sealed class ExchangeRateProvider
    {
        private const char VALUE_SEPARATOR = '|';
        private const int AMOUNT_POSITION = 2;
        private const int CODE_POSITION = 3;
        private const int RATE_POSITION = 4;
        private const int EXPECTED_VALUE_COUNT = 5;

        private readonly HttpClient httpClient;
        private readonly string exchangeRateEndpoint;
        private readonly Currency targetCurrency = new Currency("CZK");
        private readonly CultureInfo parsingCulture = new CultureInfo("en-US");

        public ExchangeRateProvider(HttpClient httpClient, string exchangeRateEndpoint)
        {
            this.httpClient = httpClient;
            this.exchangeRateEndpoint = exchangeRateEndpoint;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var currencyHashSet = new HashSet<string>(currencies.Select(c => c.Code));
            var rates = new List<ExchangeRate>();

            var response = await httpClient.GetAsync(exchangeRateEndpoint);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new ExchangeRatesSourceException("Invalid response from the source.");
            }
            var responseStream = new StreamReader(await response.Content.ReadAsStreamAsync());

            // Skip the first two lines, since they don't contain any rates
            await responseStream.ReadLineAsync();
            await responseStream.ReadLineAsync();

            while (!responseStream.EndOfStream)
            {
                var line = await responseStream.ReadLineAsync();

                var values = line.Split(VALUE_SEPARATOR);
                var exchangeRate = ParseLine(values, currencyHashSet);
                if (exchangeRate != null)
                {
                    rates.Add(exchangeRate);
                }
            }

            return rates;
        }

        private ExchangeRate ParseLine(string[] values, HashSet<string> currencies)
        {
            if (values.Length != EXPECTED_VALUE_COUNT)
            {
                throw new ExchangeRatesSourceException("Currecny line does not contain correct amount of values.");
            }

            if (!decimal.TryParse(values[AMOUNT_POSITION], NumberStyles.Number, parsingCulture, out var amount))
            {
                throw new ExchangeRatesSourceException("Cannot parse the currency amount.");
            }

            if (!decimal.TryParse(values[RATE_POSITION], NumberStyles.Number, parsingCulture, out var rate))
            {
                throw new ExchangeRatesSourceException("Cannot parse the rate.");
            }

            var sourceCurrencyCode = values[CODE_POSITION];
            if (!currencies.Contains(sourceCurrencyCode))
            {
                return null;
            }

            return new ExchangeRate(new Currency(sourceCurrencyCode), targetCurrency, rate / amount);
        }
    }
}
