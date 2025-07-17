using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string Headers = "Country|Currency|Amount|Code|Rate";
        private const int IdxCurrency = 3;
        private const int IdxAmount = 2;
        private const int IdxRate = 4;

        private static readonly Currency ExchangeProviderCurrency = new("CZK");
        
        // Ideally this would come from HttpClientFactory rather than be created like this
        private readonly HttpClient _httpClient;
        private readonly IOptions<ExchangeRateProviderSettings> _settings;

        public ExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeRateProviderSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IReadOnlyList<ExchangeRate>> GetExchangeRatesAsync(IReadOnlySet<Currency> currencies, CancellationToken cancellationToken = default)
        {
            // https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml returns an XML file
            // https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt returns a txt file
            // I used the TXT version, it is a bit less verbose wwand I assume the structure would not change often
			// so we can save importing a library to parse simple text
			// XML would make it more resistent to format changes in case they moved fields or added new ones
            if (!currencies.Any()) return new List<ExchangeRate>();
 
            // The rates are updated daily, only during working days, we could introduce some caching to prevent re-requesting the same data           
            var response = await _httpClient.GetAsync(_settings.Value.BankUrl, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            
            return await ParseExchangeRatesAsync(currencies, content);
        }

        // this code could be moved to an helper method to help with testing
        // I left it here because it simple enough and makes the class self contained
        private async Task<IReadOnlyList<ExchangeRate>> ParseExchangeRatesAsync(IReadOnlySet<Currency> currencies, string content)
        {
            using var reader = new StringReader(content);

            // skip the line with date
            string? line = await reader.ReadLineAsync();

            // header line
			line = await reader.ReadLineAsync();
            if (line == null)
            {
                throw new InvalidOperationException("Missing header line");
            }
            
            if (!line.Equals(Headers, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException($"Invalid header line. Expected: '{Headers}' Got: '{line}'");
            }

            var result = new List<ExchangeRate>();
            while ((line = await reader.ReadLineAsync()) != null)
            {
                var rate = ParseExchangeRateLine(currencies, line);
                if (rate != null)
                {
                    result.Add(rate);
                }
            }
            
            return result;
        }

        private ExchangeRate? ParseExchangeRateLine(IReadOnlySet<Currency> currencies, string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return null;
            }
            
            var parts = line.Split('|');
            if (parts.Length != 5)
            {
                throw new InvalidOperationException($"Invalid number of parts on line: {line}");
            }
            
            var currency = new Currency(parts[IdxCurrency]);
            if (!currencies.Contains(currency))
            {
                return null;
            }
            
            var amountString = parts[IdxAmount];
            var rateString = parts[IdxRate];

            if (!decimal.TryParse(rateString, out var rate))
            {
                throw new InvalidOperationException($"Unable to parse rate for line: {line}");
            }

            if (!int.TryParse(amountString, out var amount))
            {
                throw new InvalidOperationException($"Unable to parse amount for line: {line}");
            }
            
            return new ExchangeRate(currency, ExchangeProviderCurrency, rate / amount);
        }
    }
}
