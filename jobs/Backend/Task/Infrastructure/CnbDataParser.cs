using System.Globalization;
using ExchangeRateUpdater.Constants;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Infrastructure;

/// <summary>
/// Parser for CNB exchange rate data format.
/// CNB format: Country|Currency|Amount|Code|Rate
/// Example: USA|dollar|1|USD|22.950
/// </summary>
public class CnbDataParser(ILogger<CnbDataParser> logger) : ICnbDataParser
{
    private readonly ILogger<CnbDataParser> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private const char Delimiter = '|';
    private const int ExpectedColumnCount = 5;

    public IEnumerable<CnbExchangeRateDto> Parse(string rawData)
    {
        if (string.IsNullOrWhiteSpace(rawData))
        {
            _logger.LogWarning(LogMessages.CnbDataParser.EmptyOrNullData);
            return Enumerable.Empty<CnbExchangeRateDto>();
        }

        var lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        if (lines.Length < 3)
        {
            _logger.LogWarning(LogMessages.CnbDataParser.InsufficientLines);
            return Enumerable.Empty<CnbExchangeRateDto>();
        }

        // Skip first two lines (date header and column names)
        var dataLines = lines.Skip(2);
        var results = new List<CnbExchangeRateDto>();

        foreach (var line in dataLines)
        {
            try
            {
                var dto = ParseLine(line);
                if (dto != null)
                {
                    results.Add(dto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, LogMessages.CnbDataParser.FailedToParseLine, line);
                // Continue parsing other lines even if one fails
            }
        }

        _logger.LogInformation(LogMessages.CnbDataParser.ParseSuccessful, results.Count);
        return results;
    }

    private CnbExchangeRateDto? ParseLine(string line)
    {
        var parts = line.Split(Delimiter);

        if (parts.Length != ExpectedColumnCount)
        {
            _logger.LogWarning(LogMessages.CnbDataParser.UnexpectedColumnCount,
                ExpectedColumnCount, parts.Length, line);
            return null;
        }

        if (!int.TryParse(parts[2], out var amount))
        {
            _logger.LogWarning(LogMessages.CnbDataParser.FailedToParseAmount, parts[2]);
            return null;
        }

        if (!decimal.TryParse(parts[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var rate))
        {
            _logger.LogWarning(LogMessages.CnbDataParser.FailedToParseRate, parts[4]);
            return null;
        }

        return new CnbExchangeRateDto
        {
            Country = parts[0].Trim(),
            CurrencyName = parts[1].Trim(),
            Amount = amount,
            Code = parts[3].Trim(),
            Rate = rate
        };
    }
}
