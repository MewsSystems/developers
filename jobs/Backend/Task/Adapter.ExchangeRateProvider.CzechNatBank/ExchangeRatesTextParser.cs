using Adapter.ExchangeRateProvider.CzechNatBank.Dtos;
using ExchangeRateUpdater.Domain.ValueObjects;
using Serilog;
using System.Globalization;

namespace Adapter.ExchangeRateProvider.CzechNatBank;

/// <summary>
/// The class responsible for parsing txt data retrieved from Czech National Bank.
/// </summary>
internal class ExchangeRatesTextParser : IDisposable
{
    private readonly StreamReader _reader;
    private readonly ILogger _logger;

    /// <summary>
    /// Constructor to initialize the parser.
    /// </summary>
    /// <param name="reader">Stream reader that encapsulates the stream retrieved.</param>
    /// <param name="logger">Instance of <see cref="Serilog.ILogger"/></param>
    /// <exception cref="ArgumentNullException"></exception>
    public ExchangeRatesTextParser(StreamReader? reader, ILogger? logger)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Method to parse the txt data for all fx rates for a certain date.
    /// </summary>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> instance.</param>
    /// <returns>Raw exchange rates <see cref="ExchangeRateDataRawDto"></returns>
    /// <exception cref="FormatException">throws in case parser did expect a certain file format and the format was wrong.</exception>
    internal async Task<IEnumerable<ExchangeRateDataRawDto>> GetDefaultFormattedExchangeRatesAsync(CancellationToken cancellationToken)
    {
        var rawExchangeData = new List<ExchangeRateDataRawDto>();

        if (!_reader.BaseStream.CanRead) return rawExchangeData;


        // Skip date line
        var dateLine = await _reader.ReadLineAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(dateLine))
        {
            _logger.Error("Couldn't retrieve date data from Czech National Bank.");
            throw new FormatException("Couldn't retrieve date data from Czech National Bank.");
        }

        var date = DateTime.ParseExact(dateLine?.Substring(0, "dd MMM yyyy".Length) ?? string.Empty, "dd MMM yyyy", CultureInfo.InvariantCulture);

        var header = await _reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(header))
        {
            _logger.Error("Couldn't retrieve header data from Czech National Bank.");
            throw new FormatException("Couldn't retrieve header data from Czech National Bank.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        var columns = header.Split('|').ToList();

        foreach (var _ in columns) _.Trim();

        var indexOfAmount = columns.IndexOf("Amount");
        if (indexOfAmount == -1)
        {
            _logger.Error("Couldn't retrieve Amount from document.");
            throw new FormatException("Couldn't retrieve Amount from document.");
        }

        var indexOfCode = columns.IndexOf("Code");

        if (indexOfCode == -1)
        {
            _logger.Error("Couldn't retrieve Code from document.");
            throw new FormatException("Couldn't retrieve Code from document.");
        }

        var indexOfRate = columns.IndexOf("Rate");

        if (indexOfRate == -1)
        {
            _logger.Error("Couldn't retrieve Rate from document.");
            throw new FormatException("Couldn't retrieve Rate from document.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        int lineCounter = 3; // First 2 lines were already parsed.
        while (_reader.EndOfStream == false)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await _reader.ReadLineAsync();

            var lineColumns = line?.Split('|').Where(col => !string.IsNullOrWhiteSpace(col)).ToList() ?? new List<string>();
            foreach (var _ in lineColumns) _.Trim();
            if (!lineColumns.Any() || lineColumns.Count <= Math.Max(indexOfRate, Math.Max(indexOfCode, indexOfAmount)))
            {
                // Log only 10 first characters from line since the line might get too long.
                _logger.Warning("Could not parse line {LineNumber}. The line start with: {LineText}", lineCounter, line?.Substring(0, Math.Min(10, line.Length)) ?? "No content");
                continue;
            }

            rawExchangeData.Add(new ExchangeRateDataRawDto
            {
                Amount = Convert.ToInt32(lineColumns[indexOfAmount]),
                CurrencyCode = lineColumns[indexOfCode],
                Rate = Convert.ToDecimal(lineColumns[indexOfRate]),
                DateTime = date
            });

            ++lineCounter;
        }

        return rawExchangeData;
    }

    /// <summary>
    /// Get formatted data for the exchange rates.
    /// </summary>
    /// <param name="requestedCurrency">The source currency for which the fx rates need to be parsed.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken "/> instance</param>
    /// <returns>Returns raw data of source currency exchange rates <see cref="ExchangeRateDataRawDto"/></returns>
    /// <exception cref="FormatException"></exception>
    internal async Task<IEnumerable<ExchangeRateDataRawDto>> GetDefaultFormattedExchangeRatesForCurrencyAsync(Currency requestedCurrency, CancellationToken cancellationToken)
    {
        var rawExchangeData = new List<ExchangeRateDataRawDto>();

        if (!_reader.BaseStream.CanRead || _reader.EndOfStream) return rawExchangeData;

        var currencyLine = await _reader.ReadLineAsync();
        if (currencyLine == "Database does not contain any data for specified period.") return rawExchangeData;
        var currencyLineColumns = currencyLine?.Split('|').ToList() ?? new List<string>();

        if (!currencyLineColumns.Any() || currencyLineColumns.Count < 2)
        {
            _logger.Error("Couldn't not retrieve currency info line.");
            throw new FormatException("Couldn't not retrieve currency info line.");
        }

        var currencyPart = currencyLineColumns[0].Split(':');
        var amountPart = currencyLineColumns[1].Split(':');

        if (currencyPart.Count() < 2 || currencyPart[0].Trim() != "Currency" || amountPart.Count() < 2 || amountPart[0].Trim() != "Amount")
        {
            _logger.Error("Couldn't not retrieve currency code and/or amount. Line: {Line}", currencyLine?.Substring(0, 10) ?? "No content");
            throw new FormatException("Couldn't not retrieve currency code and/or amount.");
        }

        string currencyCode = currencyPart[1].Trim();

        if (currencyCode != requestedCurrency.CurrencyCode)
        {
            _logger.Error("The requested currency is different than the one parsed. Requested: {RequestedCurrency}. Retrieved: {RetrievedCurrency}", requestedCurrency, currencyCode);
            throw new FormatException($"The requested currency is different than the one parsed. Requested: {requestedCurrency.CurrencyCode}. Retrieved: {currencyCode}");
        }

        int amount = 0;
        if (!int.TryParse(amountPart[1], out amount))
        {
            _logger.Error("Couldn't not convert amount in a number. Line: {Line}", currencyLine?.Substring(0, 10));
            throw new FormatException("Couldn't not convert amount in a number.");
        }

        cancellationToken.ThrowIfCancellationRequested();

        var header = await _reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(header))
        {
            _logger.Error("Couldn't retrieve header data from Czech National Bank.");
            throw new FormatException("Couldn't retrieve header data from Czech National Bank.");
        }

        var columns = header.Split('|').ToList();

        foreach (var _ in columns) _.Trim();

        var indexOfDate = columns.IndexOf("Date");
        if (indexOfDate == -1)
        {
            _logger.Error("Couldn't retrieve Date from document.");
            throw new FormatException("Couldn't retrieve Date from document.");
        }

        var indexOfRate = columns.IndexOf("Rate");

        if (indexOfRate == -1)
        {
            _logger.Error("Couldn't retrieve Rate from document.");
            throw new FormatException("Couldn't retrieve Rate from document.");
        }
        cancellationToken.ThrowIfCancellationRequested();

        int lineCounter = 3; // First 2 lines were already parsed.
        while (_reader.EndOfStream == false)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var line = await _reader.ReadLineAsync();

            var lineColumns = line?.Split('|').Where(col => !string.IsNullOrWhiteSpace(col)).ToList() ?? new List<string>();

            foreach (var _ in lineColumns) _.Trim();

            if (!lineColumns.Any() || lineColumns.Count <= Math.Max(indexOfRate, indexOfDate))
            {
                // Log only 10 first characters from line since the line might get too long.
                _logger.Warning("Could not parse line {LineNumber}. The line start with: {LineText}", lineCounter, line?.Substring(0, 10) ?? "No content");
                continue;
            }

            rawExchangeData.Add(new ExchangeRateDataRawDto
            {
                Amount = amount,
                CurrencyCode = currencyCode,
                Rate = Convert.ToDecimal(lineColumns[indexOfRate]),
                DateTime = DateTime.ParseExact(lineColumns[indexOfDate], "dd.MM.yyyy", null),
            });

            ++lineCounter;
        }

        return rawExchangeData;
    }

    public void Dispose()
    {
        _reader.Close();
        _reader.Dispose();
    }
}
