using System;
using System.Collections.Generic;
using System.Linq;

namespace ExchangeRateUpdater.Model;

public class CnbTxtParser
{
    private static readonly char DELIMITER = '|';
    
    public static List<CnbFxRow> ParseResponse(string rawResponse)
    {
        return rawResponse
            .Split("\n")
            .Skip(2) // Header rows skipped
            .SkipLast(1) // Skip empty row at the end
            .Select(line => CnbFxRow.FromSeparatedString(line, DELIMITER))
            .ToList();
    }
}