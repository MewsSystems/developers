using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater
{
    public class CnbExchangeRateSource : IExchangeRateSource
    {
        private const int CnbHeaderLineCount = 2;

        private readonly HttpClient _httpClient;
        private readonly IOptions<CnbOptions> _options;
        private readonly ILogger<CnbExchangeRateSource> _logger;

        public CnbExchangeRateSource(
            HttpClient httpClient,
            IOptions<CnbOptions> options,
            ILogger<CnbExchangeRateSource> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;
        }

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates()
        {
            var dailyKurzUrl = _options.Value.DailyKurzUrl;
            _logger.LogDebug("Fetching CNB exchange rates from {Url}.", dailyKurzUrl);

            var response = await _httpClient.GetAsync(dailyKurzUrl).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "CNB exchange-rate request failed with status code {StatusCode}.",
                    response.StatusCode);
            }

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var rates = ParseCnbDailyKurz(content, out var skippedRowCount);

            if (skippedRowCount > 0)
            {
                _logger.LogWarning(
                    "Skipped {SkippedRowCount} malformed CNB exchange-rate rows.",
                    skippedRowCount);
            }

            _logger.LogInformation("Parsed {RateCount} CNB exchange rates.", rates.Count);
            return rates;
        }

        private IReadOnlyList<ExchangeRate> ParseCnbDailyKurz(string content, out int skippedRowCount)
        {
            skippedRowCount = 0;
            var rates = new List<ExchangeRate>();

            if (string.IsNullOrWhiteSpace(content))
            {
                return rates;
            }

            var lines = content.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            if (lines.Length <= CnbHeaderLineCount)
            {
                return rates;
            }

            for (var i = CnbHeaderLineCount; i < lines.Length; i++)
            {
                var line = lines[i].Trim();
                if (line.Length == 0)
                {
                    continue;
                }

                var parts = line.Split('|');
                if (parts.Length < 5)
                {
                    skippedRowCount++;
                    LogSkippedRow(i + 1, "expected at least 5 pipe-delimited columns", line);
                    continue;
                }

                var amountText = parts[2].Trim();
                var code = parts[3].Trim();
                var rateText = parts[4].Trim();

                if (string.IsNullOrEmpty(code))
                {
                    skippedRowCount++;
                    LogSkippedRow(i + 1, "missing currency code", line);
                    continue;
                }

                if (!decimal.TryParse(amountText, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount)
                    || amount <= 0)
                {
                    skippedRowCount++;
                    LogSkippedRow(i + 1, $"invalid amount '{amountText}'", line);
                    continue;
                }

                if (!TryParseCnbDecimal(rateText, out var rate))
                {
                    skippedRowCount++;
                    LogSkippedRow(i + 1, $"invalid rate '{rateText}'", line);
                    continue;
                }

                var value = rate / amount;
                rates.Add(new ExchangeRate(new Currency(code), new Currency("CZK"), value));
            }

            return rates;
        }

        private void LogSkippedRow(int lineNumber, string reason, string row)
        {
            _logger.LogDebug(
                "Skipping malformed CNB exchange-rate row at line {LineNumber}: {Reason}. Row: {Row}",
                lineNumber,
                reason,
                row);
        }

        private static bool TryParseCnbDecimal(string text, out decimal value)
        {
            // Avoid AllowThousands: CNB English files use "." as decimal; a comma must not be read as a thousands separator.
            const NumberStyles rateStyles = NumberStyles.Number & ~NumberStyles.AllowThousands;

            if (decimal.TryParse(text, rateStyles, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            // Czech-published files often use "," as the decimal separator in this column.
            var withDotDecimal = text.Replace(',', '.');
            if (decimal.TryParse(withDotDecimal, rateStyles, CultureInfo.InvariantCulture, out value))
            {
                return true;
            }

            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.GetCultureInfo("cs-CZ"), out value);
        }
    }
}