using System.Globalization;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Exceptions;
using ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Models;

namespace ExchangeRateUpdater.Infrastructure.Providers.CzechNationalBank.Parsers;

/// <summary>
/// Parses exchange rate data returned by the Czech National Bank daily rates endpoint.
/// </summary>
/// <remarks>
/// Expected format:
/// <code>
/// 23 Dec 2025 #248
/// Country|Currency|Amount|Code|Rate
/// Australia|dollar|1|AUD|13.818
/// </code>
/// </remarks>
internal sealed class PipeSeparatedDailyExchangeResponseParser : IDailyExchangeRatesResponseParser
{
    private static readonly char[] _newLineCharacters = ['\r', '\n'];
    private const int _expectedColumnCount = 5;
    private const int _minimumLineCount = 3;
    private const int _expectedHeaderPartsCount = 2;
    private const string _expectedHeaderColumns = "Country|Currency|Amount|Code|Rate";
    public DailyExchangeRatesResponse Parse(string rawData)
    {
        if (string.IsNullOrWhiteSpace(rawData))
        {
            throw new CzechNationalBankParsingException("Raw data is empty, null or white space.");
        }

        var contents = GetContents(rawData);

        var header = contents[0];
        var headerParts = GetHeaderParts(header);
        var exchangeDate = GetExchangeDate(headerParts);
        var exchangeSequence = GetExchangeSequence(headerParts);

        ValidateColumnNames(contents[1]);

        return new DailyExchangeRatesResponse
        (
            Date: exchangeDate,
            Sequence: exchangeSequence,
            ExchangeRates: contents[2..]
                .Select(ParseRecord)
                .ToArray()
        );
    }

    private static string[] GetContents(string rawData)
    {
        var contents = rawData.Split(_newLineCharacters, StringSplitOptions.RemoveEmptyEntries);
        if (contents.Length < _minimumLineCount)
        {
            throw new CzechNationalBankParsingException($"Response does not contain enough lines. Lines found: {contents.Length}.");
        }

        return contents;
    }

    private static string[] GetHeaderParts(string header)
    {
        var headerParts = header.Split('#', StringSplitOptions.TrimEntries);

        if (headerParts.Length != _expectedHeaderPartsCount)
        {
            throw new CzechNationalBankParsingException($"Header is not in expected format. Header value: '{header}'.");
        }

        return headerParts;
    }

    private static DateOnly GetExchangeDate(string[] headerParts)
    {
        var value = headerParts[0];
        if (!DateOnly.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
        {
            throw new CzechNationalBankParsingException($"Header date is not in expected format. Value: '{value}'.");
        }

        return date;
    }

    private static int GetExchangeSequence(string[] headerParts)
    {
        var value = headerParts[1];
        if (!int.TryParse(value, out var sequence))
        {
            throw new CzechNationalBankParsingException($"Header sequence is not in expected format. Value: '{value}'.");
        }

        return sequence;
    }

    private static void ValidateColumnNames(string columnNames)
    {
        if (!string.Equals(columnNames.Trim(), _expectedHeaderColumns, StringComparison.OrdinalIgnoreCase))
        {
            throw new CzechNationalBankParsingException($"Column names are not in expected format. Value: '{columnNames}'.");
        }
    }

    private static ExchangeRate ParseRecord(string line)
    {
        var parts = line.Split('|', StringSplitOptions.TrimEntries);

        if (parts.Length != _expectedColumnCount)
        {
            throw new CzechNationalBankParsingException($"Invalid record format: '{line}'. Expected {_expectedColumnCount} pipe-separated columns.");
        }

        var amountPart = parts[2];
        if (!int.TryParse(amountPart, out var amount))
        {
            throw new CzechNationalBankParsingException($"Invalid amount: '{amountPart}'");
        }

        var ratePart = parts[4];
        if (!decimal.TryParse(ratePart, NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
        {
            throw new CzechNationalBankParsingException($"Invalid rate: '{ratePart}'");
        }

        return new ExchangeRate(
            Country: parts[0],
            CurrencyName: parts[1],
            Amount: amount,
            Code: parts[3],
            Rate: rate
        );
    }
}