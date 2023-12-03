using Adapter.ExchangeRateProvider.CzechNationalBank.Dtos;
using ExchangeRateUpdater.Domain.Entities;
using Serilog;
using System.Reflection.PortableExecutable;

namespace Adapter.ExchangeRateProvider.CzechNationalBank;

internal class ExchangeRatesTextParser : IDisposable
{
    private readonly StreamReader _reader;
    private readonly ILogger _logger;

    public ExchangeRatesTextParser(StreamReader? reader, ILogger? logger)
    {
        _reader = reader ?? throw new ArgumentNullException(nameof(reader));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    internal async Task<IEnumerable<ExchangeDataRawDto>> GetDefaultFormattedExchangeRatesAsync()
    {
        var rawExchangeData = new List<ExchangeDataRawDto>();

        if (!_reader.BaseStream.CanRead) return rawExchangeData;


        // Skip date line
        await _reader.ReadLineAsync();

        var header = await _reader.ReadLineAsync();

        if (string.IsNullOrWhiteSpace(header))
        {
            _logger.Error("Couldn't retrieve header data from Czech National Bank.");
            throw new FormatException("Couldn't retrieve header data from Czech National Bank.");
        }

        var columns = header.Split('|').ToList();

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

            if (!lineColumns.Any() || lineColumns.Count <= Math.Max(indexOfRate, Math.Max(indexOfCode, indexOfAmount)))
            {
                // Log only 10 first characters from line since the line might get too long.
                _logger.Warning("Could not parse line {LineNumber}. The line start with: {LineText}", lineCounter, line?.Substring(0, 10) ?? "No content");
                continue;
            }

            rawExchangeData.Add(new ExchangeDataRawDto
            {
                Amount       = Convert.ToInt32(lineColumns[indexOfAmount]),
                CurrencyCode = lineColumns[indexOfCode],
                Rate         = Convert.ToDecimal(lineColumns[indexOfRate]) 
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
