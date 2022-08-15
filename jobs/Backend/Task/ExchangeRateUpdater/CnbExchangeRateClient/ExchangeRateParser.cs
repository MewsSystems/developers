using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateParser : IExchangeRateParser
{
    public async Task<IEnumerable<ExchangeRateLine>> ParseExchangeRateList(Stream stream)
    {
        using var streamReader = new StreamReader(stream);

        // There are 2 header lines in the list which we need to skip
        int headerLines = 2;
        var list = new List<ExchangeRateLine>();
        while (await streamReader.ReadLineAsync() is { } line)
            if (headerLines-- <= 0) 
                list.Add(ParseLine(line));

        return list;
    }
    
    private ExchangeRateLine ParseLine(string line)
    {
        var segments = line.Split('|');
        if (segments.Length != 5)
        {
            throw new Exception($"An exchange rate line should have 5 segments, found {segments.Length} instead.");
        }

        if (!decimal.TryParse(segments[2], NumberStyles.Any, new CultureInfo("cs-CZ"), out var amount))
        {
            throw new Exception("Amount is not a valid number");
        }

        if (!decimal.TryParse(segments[4], NumberStyles.Any, new CultureInfo("cs-CZ"), out var exchangeRate))
        {
            throw new Exception("Exchange rate is not a valid number");
        }

        return new ExchangeRateLine(
            segments[0], segments[1], amount, segments[3], exchangeRate);
    }
}