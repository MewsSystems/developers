using System.Globalization;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Clients.Cnb.Parsers;

/// <summary>
/// Cnb client response parser.
/// </summary>
public class CnbClientResponseParser : ICnbClientResponseParser
{
    private const char LineDelimiter = '|';
    private const char DateDelimiter = '#';
    private const string CurrencyDecimalSeparator = ".";
    private const int LineColumnsLength = 5;

    private readonly ILogger<CnbClientResponseParser> _logger;

    /// <summary>
    /// Constructs a <see cref="CnbClientResponseParser"/>
    /// </summary>
    /// <param name="logger">The logger.</param>
    public CnbClientResponseParser(ILogger<CnbClientResponseParser> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Extracts each line by defined specs.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns an exchange rate.</returns>
    public ExchangeRateDto? ExtractExchangeRate(string line)
    {
        ExchangeRateDto? exchangeRate = null;

        var lineColumns = line.Split(LineDelimiter);

        if (lineColumns.Length != LineColumnsLength) return exchangeRate;

        try
        {
            exchangeRate = new ExchangeRateDto
            {
                Country = lineColumns[0],
                Currency = lineColumns[1],
                Amount = Convert.ToInt32(lineColumns[2]),
                Code = lineColumns[3],
                Rate = Convert.ToDecimal(lineColumns[4], new NumberFormatInfo { CurrencyDecimalSeparator = CurrencyDecimalSeparator })
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);
        }

        return exchangeRate;
    }

    /// <summary>
    /// Extracts date by defined specs.
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns a date.</returns>
    public DateTime? ExtractDate(string line)
    {
        DateTime? result = null;
        
        var date = line.Split(DateDelimiter);
        
        if (DateTime.TryParse(date[0], out var dateTime))
        {
            result = dateTime;
        }

        return result;
    }
}