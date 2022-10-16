using System.Globalization;
using ExchangeRateUpdater.Clients.Cnb.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Clients.Cnb.Parsers;

/// <summary>
/// Cnb client response parser
/// </summary>
public class CnbClientResponseParser
{
    private const char LineDelimiter = '|';
    private const string CurrencyDecimalSeparator = ".";
    private const int LineLength = 5;

    private readonly ILogger<CnbClientResponseParser> _logger;

    /// <summary>
    /// Constructs a <see cref="CnbClientResponseParser"/>
    /// </summary>
    /// <param name="logger">Exception logger</param>
    public CnbClientResponseParser(ILogger<CnbClientResponseParser> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Extracts each line by defined specs
    /// </summary>
    /// <param name="line"></param>
    /// <returns>Returns an exchange rate</returns>
    public ExchangeRateDto? ExtractExchangeRate(string line)
    {
        ExchangeRateDto? exchangeRate = null;

        var splintedLine = line.Split(LineDelimiter);

        if (splintedLine.Length != LineLength) return exchangeRate;

        try
        {
            exchangeRate = new ExchangeRateDto
            {
                Country = splintedLine[0],
                Currency = splintedLine[1],
                Amount = Convert.ToInt32(splintedLine[2]),
                Code = splintedLine[3],
                Rate = Convert.ToDecimal(splintedLine[4], new NumberFormatInfo { CurrencyDecimalSeparator = CurrencyDecimalSeparator })
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{ErrorMessage}", ex.Message);
        }

        return exchangeRate;
    }
}