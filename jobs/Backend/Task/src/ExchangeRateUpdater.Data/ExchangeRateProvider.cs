using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using ExchangeRateUpdater.BusinessLogic.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Data
{
    /// <summary>
    /// Exchange rate provider that fetches rates from CNB (Czech National Bank).
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private const string DailyRatesUrl = "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt";

        public ExchangeRateProvider(HttpClient httpClient, ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            // Synchronous wrapper around async operation
            var result = GetExchangeRatesAsync(currencies).GetAwaiter().GetResult();
            return result;
        }

        private async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            try
            {
                _logger.LogDebug("Downloading daily exchange rates from {Url}", DailyRatesUrl);

                using var res = await _httpClient.GetAsync(DailyRatesUrl, HttpCompletionOption.ResponseHeadersRead);
                res.EnsureSuccessStatusCode();
                await using var stream = await res.Content.ReadAsStreamAsync();
                using var sr = new StreamReader(stream);

                var allText = await sr.ReadToEndAsync();

                return ParseRates(allText, currencies);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to download or parse exchange rates.");
                throw;
            }
        }

        /// <summary>
        /// Parses exchange rates from the source content.
        /// </summary>
        public IEnumerable<ExchangeRate> ParseRates(string content, IEnumerable<Currency> currencies)
        {
            var lines = content.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 3)
                return Enumerable.Empty<ExchangeRate>();

            var codesInInput = new HashSet<string>(currencies.Select(c => c.Code), StringComparer.OrdinalIgnoreCase);
            var targetCurrencyCode = "CZK";

            var result = new List<ExchangeRate>();

            try
            {
                // The CNB data format:
                // Line 1: Date (e.g., "14 Nov 2025")
                // Line 2: Reference number (e.g., "#222")
                // Line 3: Header (e.g., "Country|Currency|Amount|Code|Rate")
                // Line 4+: Data rows
                
                var csvLines = lines.Skip(2).ToList();
                
                if (csvLines.Count == 0)
                    return Enumerable.Empty<ExchangeRate>();

                var csvContent = string.Join(Environment.NewLine, csvLines);

                using var reader = new StringReader(csvContent);
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = "|",
                    HasHeaderRecord = true,
                    HeaderValidated = null,  // Disable header validation
                };
                using var csv = new CsvReader(reader, config);
                
                // Register the class map for proper field mapping
                csv.Context.RegisterClassMap<ExchangeRateRecordMap>();

                // Read all records - materialize the enumerable to ensure parsing happens
                var records = csv.GetRecords<ExchangeRateRecord>().ToList();

                foreach (var record in records)
                {
                    try
                    {
                        // Skip records with invalid data
                        if (record?.Code == null || string.IsNullOrWhiteSpace(record.Code))
                            continue;

                        // Validate that both source currency and target currency (CZK) are requested
                        if (!codesInInput.Contains(record.Code) || !codesInInput.Contains(targetCurrencyCode))
                            continue;

                        // Calculate unit rate: rate / amount
                        var unitRate = record.Rate / record.Amount;
                        var sourceCurrency = new Currency(record.Code);
                        var targetCurrency = new Currency(targetCurrencyCode);

                        result.Add(new ExchangeRate(sourceCurrency, targetCurrency, unitRate));
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to parse record for currency code.");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse CSV content.");
            }

            return result;
        }

        /// <summary>
        /// CSV record mapping for exchange rate data from CNB.
        /// </summary>
        private class ExchangeRateRecord
        {
            public string Country { get; set; }
            public string Currency { get; set; }
            public decimal Amount { get; set; }
            public string Code { get; set; }
            public decimal Rate { get; set; }
        }

        /// <summary>
        /// Class map for CsvHelper to properly map CSV columns to ExchangeRateRecord properties.
        /// Maps by column index (0-based) instead of by header name for better flexibility.
        /// </summary>
        private sealed class ExchangeRateRecordMap : ClassMap<ExchangeRateRecord>
        {
            public ExchangeRateRecordMap()
            {
                // Map by index instead of header name for more flexibility
                Map(m => m.Country).Index(0);
                Map(m => m.Currency).Index(1);
                Map(m => m.Amount).Index(2);
                Map(m => m.Code).Index(3);
                Map(m => m.Rate).Index(4);
            }
        }
    }
}
