using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using ExchangeRateUpdater.Options;
using ExchangeRateUpdater.Services.Interfaces;
using System.Linq;

namespace ExchangeRateUpdater.Services.CNB
{
    public class CNBRateParser : IExchangeRateParser
    {
        private readonly ILogger<CNBRateParser> _logger;
        private readonly HashSet<string> _validIsoCodes;
        private static readonly CultureInfo ParsingCulture = CultureInfo.InvariantCulture;

        public CNBRateParser(
            IOptions<ExchangeRateOptions> options,
            ICurrencyIsoService currencyIsoService,
            ILogger<CNBRateParser> logger)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));
            if (currencyIsoService == null)
                throw new ArgumentNullException(nameof(currencyIsoService));
            
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _validIsoCodes = currencyIsoService.GetValidIsoCodes();
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Currency code cannot be empty or whitespace.", nameof(code));
            }

            if (code.Length != 3)
            {
                throw new ArgumentException("Currency code must be exactly 3 characters long.", nameof(code));
            }

            if (!code.All(char.IsLetter))
            {
                throw new ArgumentException("Currency code must contain only letters.", nameof(code));
            }

            var normalizedCode = code.ToUpperInvariant();
            if (!_validIsoCodes.Contains(normalizedCode))
            {
                throw new ArgumentException($"'{normalizedCode}' is not a valid ISO 4217 currency code.", nameof(code));
            }
        }

        public IEnumerable<(string Code, decimal Amount, decimal Rate)> ParseRates(string data)
        {
            var result = new Dictionary<string, (string Code, decimal Amount, decimal Rate)>();
            var lines = data.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (lines.Length < 2)
            {
                _logger.LogError("Rate file does not contain enough lines (date and header required)");
                return result.Values;
            }

            _logger.LogDebug("Processing {Count} exchange rates...", lines.Length - 2);

            var processedRates = 0;
            var invalidRates = 0;
            var duplicateRates = 0;

            for (int i = 2; i < lines.Length; i++)
            {
                var parts = lines[i].Split('|', 5);
                if (parts.Length == 5)
                {
                    ReadOnlySpan<char> amountSpan = parts[2].AsSpan().Trim();
                    ReadOnlySpan<char> rateSpan = parts[4].AsSpan().Trim();
                    var code = parts[3].Trim();

                    try
                    {
                        ValidateCode(code);
                    }
                    catch (ArgumentException ex)
                    {
                        _logger.LogWarning(ex, "Invalid currency code at line {LineNumber}", i + 1);
                        invalidRates++;
                        continue;
                    }

                    if (result.ContainsKey(code))
                    {
                        _logger.LogWarning("Duplicate currency code found and skipped: {Code} at line {LineNumber}", code, i + 1);
                        duplicateRates++;
                        continue;
                    }

                    if (!decimal.TryParse(amountSpan, NumberStyles.Number, ParsingCulture, out decimal amount))
                    {
                        _logger.LogWarning("Invalid amount format for currency {Code}: {Amount}", code, amountSpan.ToString());
                        invalidRates++;
                        continue;
                    }

                    if (!decimal.TryParse(rateSpan, NumberStyles.Number, ParsingCulture, out decimal rate))
                    {
                        _logger.LogWarning("Invalid rate format for currency {Code}: {Rate}", code, rateSpan.ToString());
                        invalidRates++;
                        continue;
                    }

                    result[code] = (code, amount, rate);
                    processedRates++;
                }
                else
                {
                    _logger.LogWarning("Invalid line format at line {LineNumber}: {Line}", i + 1, lines[i]);
                    invalidRates++;
                }
            }

            _logger.LogInformation("Process completed: {ProcessedRates} rates processed, {InvalidRates} invalid, {DuplicateRates} duplicates found",
                processedRates, invalidRates, duplicateRates);

            return result.Values;
        }
    }
} 