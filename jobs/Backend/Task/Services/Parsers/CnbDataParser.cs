using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Errors;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services.Handlers;
using FluentResults;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Linq;

namespace ExchangeRateUpdater.Services.Parsers;

public class CnbDataParser : ICnbDataParser
{
    private readonly ILogger<CnbDataParser> _logger;

    public CnbDataParser(ILogger<CnbDataParser> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Result<CnbExchangeRateData> Parse(string rawData)
    {
        _logger.LogDebug("Starting to parse CNB data");

        if (string.IsNullOrWhiteSpace(rawData))
        {
            return ErrorHandler.Handle<CnbExchangeRateData>(CnbErrorCode.EmptyResponse, "Empty response from CNB");
        }

        var lines = rawData.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
        _logger.LogDebug("Parsing {LineCount} lines of CNB data", lines.Length);

        if (lines.Length < CnbConstants.ExpectedMinimumLines)
        {
            _logger.LogError("Insufficient data: expected at least {Expected} lines, got {Actual}", CnbConstants.ExpectedMinimumLines, lines.Length);
            return ErrorHandler.Handle<CnbExchangeRateData>(CnbErrorCode.InsufficientData,
                $"Expected at least {CnbConstants.ExpectedMinimumLines} lines, got {lines.Length}");
        }

        var result = new CnbExchangeRateData
        {
            Date = ParseDate(lines[0])
        };
        _logger.LogDebug("Parsed date: {Date}", result.Date);

        ValidateHeader(lines[1]);

        for (int i = 2; i < lines.Length; i++)
        {
            if (TryParseExchangeRateEntry(lines[i], out var entry))
            {
                result.Rates.Add(entry);
            }
        }

        _logger.LogInformation("Successfully parsed {Count} exchange rates", result.Rates.Count);

        if (!result.Rates.Any())
        {
            _logger.LogError("No valid exchange rates found in CNB data");
            return ErrorHandler.Handle<CnbExchangeRateData>(CnbErrorCode.NoValidRates,
                "No valid exchange rates found in CNB data");
        }

        return Result.Ok(result);
    }

    private static DateTime ParseDate(string dateLine)
    {
        var parts = dateLine.Split(' ');
        if (parts.Length < CnbConstants.MinimumDateParts)
        {
            throw new CnbException(CnbErrorCode.InvalidDateFormat, dateLine);
        }

        var dateString = $"{parts[0]} {parts[1]} {parts[2]}";
        if (!DateTime.TryParseExact(dateString, CnbConstants.DateFormat,
            CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            throw new CnbException(CnbErrorCode.InvalidDateFormat, dateString);
        }

        return date;
    }

    private static void ValidateHeader(string headerLine)
    {
        if (!headerLine.Contains(CnbConstants.FieldSeparator) ||
                !headerLine.Contains("Country") ||
                !headerLine.Contains("Rate"))
        {
            throw new CnbException(CnbErrorCode.InvalidHeaderFormat, headerLine);
        }
    }

    private static bool TryParseExchangeRateEntry(string line, out CnbExchangeRateEntry entry)
    {
        entry = null;
        var parts = line.Split(CnbConstants.FieldSeparator);

        if (parts.Length != CnbConstants.ExpectedFieldCount)
        {
            return false;
        }

        try
        {
            entry = new CnbExchangeRateEntry
            {
                Country = parts[0].Trim(),
                Currency = parts[1].Trim(),
                Amount = int.Parse(parts[2].Trim(), CultureInfo.InvariantCulture),
                Code = parts[3].Trim(),
                Rate = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture)
            };

            return IsValidEntry(entry);
        }
        catch
        {
            entry = null;
            return false;
        }
    }

    private static bool IsValidEntry(CnbExchangeRateEntry entry)
    {
        return !string.IsNullOrWhiteSpace(entry.Code) &&
               entry.Code.Length == 3 &&
               entry.Amount > 0 &&
               entry.Rate > 0;
    }
}
