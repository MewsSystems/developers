using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace ExchangeRateUpdater.Infrastructure
{
    internal sealed class CNBExchangeRateProvider(HttpClient httpClient) : IExchangeRateProvider
    {
        private const string RatesSegment = "daily.txt";

        private readonly HttpClient httpClient = httpClient;

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Prepare the request URL (always fetch latest for now)            
            var response = await httpClient.GetAsync(RatesSegment);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            // Parse the CNB file
            var lines = content.Split('\n');
            if (lines.Length < 2)
                return new List<ExchangeRate>();

            // The first line is the header, the second line is column names, then data
            var rates = new List<ExchangeRate>();
            var currencySet = new HashSet<string>(currencies.Select(c => c.Code), System.StringComparer.OrdinalIgnoreCase);
            foreach (var line in lines.Skip(2))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed))
                    continue;
                var parts = trimmed.Split('|');
                if (parts.Length < 5)
                    continue;
                // Format: Country|Currency|Amount|Code|Rate
                var code = parts[3].Trim();
                if (!currencySet.Contains(code) && !currencySet.Contains("CZK"))
                    continue;
                if (!decimal.TryParse(parts[2], out var amount))
                    continue;
                if (!decimal.TryParse(parts[4].Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    continue;
                // CNB rates are always per 1 CZK to X foreign currency
                // So, 1 CZK = rate/amount X
                // We want CZK -> X (as provided by CNB)
                if (currencySet.Contains("CZK") && currencySet.Contains(code))
                {
                    rates.Add(new ExchangeRate(new Currency("CZK"), new Currency(code), rate / amount));
                }
            }
            return rates;
        }
    }
}
