using ExchangeRateUpdater.Models;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.ExchangeProviders
{
    public class CNBParser : ICNBParser
    {
        private readonly ILogger<CNBParser> _logger;

        public CNBParser(ILogger<CNBParser> logger)
        {
            _logger = logger;
        }

        public Result<ExchangeRates> Parse(string data, IEnumerable<Currency> currencies)
        {
            var lines = data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            if (lines.Length < 3)
            {
                _logger.LogWarning($"Exchange data contains {lines.Length} lines. It should have at least 3.");
                return Result.Fail("Input data is invalid.");
            }

            var date = GetDate(lines[0]);

            if (date is null)
            {
                return Result.Fail("Input data is invalid.");
            }

            var result = new List<ExchangeRate>();

            foreach (var item in lines.Skip(2))
            {
                var line = GetExchangeRate(item);
                if (line is { }) result.Add(line);
            }

            result = result.Where(rate => currencies.Any(currency => currency.Code == rate.TargetCurrency.Code)).ToList();

            return Result.Ok<ExchangeRates>(new ExchangeRates(result, date.GetValueOrDefault()));
        }

        private ExchangeRate GetExchangeRate(string data)
        {
            var split = data.Split('|');

            if (split.Length != 5)
            {
                _logger.LogWarning($"Line '{data}' should contain 5 segments divided by '|'.");
                return null;
            }

            if (!decimal.TryParse(split[4],NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = "," }, out var value))
            {
                _logger.LogWarning($"Value '{split[4]}' should should be a number.");
                return null;
            }

            return new ExchangeRate(new Currency(), new Currency(split[3]), value);
        }

        private DateTimeOffset? GetDate(string data)
        {
            var split = data.Split(' ');
            if (split.Length != 2)
            {
                _logger.LogWarning($"Line '{data}' should contain a date and a number divided by space.");
                return null;
            }

            if (!DateTimeOffset.TryParse(split[0], out var date))
            {
                _logger.LogWarning($"Value '{split[0]}' should be a date.");
                return null;
            }

            return date;
        }
    }
}
