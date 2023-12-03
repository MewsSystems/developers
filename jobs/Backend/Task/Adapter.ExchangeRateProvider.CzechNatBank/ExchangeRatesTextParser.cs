using Adapter.ExchangeRateProvider.CzechNatBank.Dtos;
using ExchangeRateUpdater.Domain.ValueObjects;
using Serilog;

namespace Adapter.ExchangeRateProvider.CzechNatBank;

internal class ExchangeRatesTextParser : IDisposable
{
    private readonly StreamReader _reader;
    private readonly ILogger _logger;

    public ExchangeRatesTextParser(StreamReader? reader, ILogger? logger)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    internal async Task<IEnumerable<ExchangeRateDataRawDto>> GetDefaultFormattedExchangeRatesAsync()
    {
        var rawExchangeData = new List<ExchangeRateDataRawDto>();

        if (!_reader.BaseStream.CanRead) return rawExchangeData;


        // Skip date line
        var value = await _reader.ReadLineAsync();

        var header = await _reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(header))
        {
            _logger.Error("Couldn't retrieve header data from Czech National Bank.");
            throw new FormatException("Couldn't retrieve header data from Czech National Bank.");
        }

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

        int lineCounter = 3; // First 2 lines were already parsed.
        while (_reader.EndOfStream == false)
        {
            var line = await _reader.ReadLineAsync();

            var lineColumns = line?.Split('|').ToList() ?? new List<string>();
            foreach (var _ in lineColumns) _.Trim();
            if (!lineColumns.Any() || lineColumns.Count <= Math.Max(indexOfRate, Math.Max(indexOfCode, indexOfAmount)))
            {
                // Log only 10 first characters from line since the line might get too long.
                _logger.Warning("Could not parse line {LineNumber}. The line start with: {LineText}", lineCounter, line?.Substring(0, 10) ?? "No content");
                continue;
            }

            rawExchangeData.Add(new ExchangeRateDataRawDto
            {
                Amount = Convert.ToInt32(lineColumns[indexOfAmount]),
                CurrencyCode = lineColumns[indexOfCode],
                Rate = Convert.ToDecimal(lineColumns[indexOfRate])
            });

            ++lineCounter;
        }

        return rawExchangeData;
    }

    internal async Task<IEnumerable<ExchangeDateRateDataRawDto>> GetDefaultFormattedExchangeRatesForCurrencyAsync(Currency requestedCurrency)
    {
        var rawExchangeData = new List<ExchangeDateRateDataRawDto>();

        if (!_reader.BaseStream.CanRead) return rawExchangeData;

        var currencyLine = await _reader.ReadLineAsync();
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
            throw new FormatException("The requested currency is different than the one parsed. Requested: {RequestedCurrency}. Retrieved: {RetrievedCurrency}");
        }

        int amount = 0;
        if (!int.TryParse(amountPart[1], out amount))
        {
            _logger.Error("Couldn't not convert amount in a number. Line: {Line}", currencyLine?.Substring(0, 10));
            throw new FormatException("Couldn't not convert amount in a number.");
        }

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

        int lineCounter = 3; // First 2 lines were already parsed.
        while (_reader.EndOfStream == false)
        {
            var line = await _reader.ReadLineAsync();

            var lineColumns = line?.Split('|').ToList() ?? new List<string>();

            foreach (var _ in lineColumns) _.Trim();

            if (!lineColumns.Any() || lineColumns.Count <= Math.Max(indexOfRate, indexOfDate))
            {
                // Log only 10 first characters from line since the line might get too long.
                _logger.Warning("Could not parse line {LineNumber}. The line start with: {LineText}", lineCounter, line?.Substring(0, 10) ?? "No content");
                continue;
            }

            rawExchangeData.Add(new ExchangeDateRateDataRawDto
            {
                Amount = amount,
                CurrencyCode = currencyCode,
                Rate = Convert.ToDecimal(lineColumns[indexOfRate]),
                DateTime = Convert.ToDateTime(lineColumns[indexOfDate]),
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
