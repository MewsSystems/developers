using ExchangeRateUpdater.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Providers
{
    public class CnbExchangeRateProvider : IExchangeRateProvider
    {
        private const string DailyRatesUrl =
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/" +
            "central-bank-exchange-rate-fixing/daily.txt";

        private static readonly HttpClient HttpClient = new HttpClient();

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            if (currencies == null)
                throw new ArgumentNullException(nameof(currencies));

            var raw = DownloadDailyRatesAsync().GetAwaiter().GetResult();

            var rows = ParseDailyRates(raw);

            return MapToExchangeRates(rows, currencies);
        }


        /// <summary>
        /// Downloads the daily exchange rates TXT file from the CNB public endpoint.
        /// </summary>
        /// <exception cref="HttpRequestException">
        /// Thrown when the CNB endpoint is unavailable or returns an invalid response.
        /// </exception>
        private async Task<string> DownloadDailyRatesAsync()
        {
            try
            {
                using var response = await HttpClient.GetAsync(DailyRatesUrl);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrWhiteSpace(content))
                    throw new InvalidOperationException("Received empty response from CNB endpoint.");

                return content;
            }
            catch (Exception ex)
            {
                throw new HttpRequestException("Failed to download daily exchange rates from CNB.", ex);
            }
        }

        /// <summary>
        /// Parses the raw CNB daily rates TXT content into a collection of CNB rate rows.
        /// </summary>
        /// <param name="rawContent">Raw TXT response returned by the CNB endpoint.</param>
        /// <returns>Parsed list of CNB rate rows.</returns>
        /// <exception cref="FormatException">
        /// Thrown when the response does not match the expected CNB TXT format.
        /// </exception>
        private IReadOnlyList<CnbRateRow> ParseDailyRates(string rawContent)
        {
            if (string.IsNullOrWhiteSpace(rawContent))
                throw new FormatException("CNB daily rates response is empty.");

            var lines = rawContent
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Expect at least: 1 line with date, 1 header line, and at least 1 data line
            if (lines.Length < 3)
                throw new FormatException("CNB daily rates response does not contain expected header and data lines.");

            // line[0] = date metadata, line[1] = header, rest = data
            var dataLines = lines.Skip(2);

            var result = new List<CnbRateRow>();

            foreach (var line in dataLines)
            {
                var parts = line.Split('|');

                if (parts.Length != 5)
                    throw new FormatException($"Unexpected CNB daily rates line format: '{line}'.");

                // parts[0] = Country (ignored)
                // parts[1] = Currency name (ignored)
                // parts[2] = Amount
                // parts[3] = Code
                // parts[4] = Rate

                if (!int.TryParse(parts[2], NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount))
                    throw new FormatException($"Invalid amount value in CNB daily rates line: '{line}'.");

                if (!decimal.TryParse(parts[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
                    throw new FormatException($"Invalid rate value in CNB daily rates line: '{line}'.");

                var code = parts[3].Trim();

                if (string.IsNullOrEmpty(code))
                    throw new FormatException($"Missing currency code in CNB daily rates line: '{line}'.");

                result.Add(new CnbRateRow(code, amount, rate));
            }

            return result;
        }

        /// <summary>
        /// Maps parsed CNB rate rows to domain exchange rates for the requested currencies.
        /// </summary>
        /// 
        /// <param name="rows">Parsed CNB daily rate rows.</param>
        /// <param name="requestedCurrencies">
        /// Currencies for which exchange rates should be returned. 
        /// Only currencies available in the CNB data source are included.
        /// </param>
        /// <returns>
        /// Collection of exchange rates where source currency is the requested currency and target currency is CZK.
        /// </returns>
        private IEnumerable<ExchangeRate> MapToExchangeRates(
            IReadOnlyList<CnbRateRow> rows,
            IEnumerable<Currency> requestedCurrencies)
        {
            if (rows == null)
                throw new ArgumentNullException(nameof(rows));

            if (requestedCurrencies == null)
                throw new ArgumentNullException(nameof(requestedCurrencies));

            // Normalize requested currencies by code (case-insensitive)
            var requestedByCode = requestedCurrencies
                .Where(c => c != null && !string.IsNullOrWhiteSpace(c.Code))
                .GroupBy(c => c.Code.ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First());

            if (requestedByCode.Count == 0)
                return Enumerable.Empty<ExchangeRate>();

            // Index CNB rows by currency code
            var rowsByCode = rows
                .GroupBy(r => r.Code.ToUpperInvariant())
                .ToDictionary(g => g.Key, g => g.First());

            var result = new List<ExchangeRate>();

            // Target currency is always CZK for CNB rates
            var targetCurrency = new Currency("CZK");

            foreach (var (codeKey, sourceCurrency) in requestedByCode)
            {
                if (!rowsByCode.TryGetValue(codeKey, out var row))
                {
                    // CNB does not provide rate for this currency – we skip it.
                    continue;
                }

                if (row.Amount <= 0)
                    throw new FormatException($"Invalid amount '{row.Amount}' for currency '{row.Code}' in CNB data.");

                var valuePerUnit = row.Rate / row.Amount;

                result.Add(new ExchangeRate(
                    sourceCurrency,
                    targetCurrency,
                    valuePerUnit));
            }

            return result;
        }


        private sealed class CnbRateRow
        {
            public CnbRateRow(string code, int amount, decimal rate)
            {
                Code = code;
                Amount = amount;
                Rate = rate;
            }

            public string Code { get; }
            public int Amount { get; }
            public decimal Rate { get; }
        }
    }
}
