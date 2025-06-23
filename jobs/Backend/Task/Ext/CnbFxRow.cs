using System;
using System.Globalization;

namespace ExchangeRateUpdater.Model;

public record CnbFxRow(string Country, string CurrencyStr, long Amount, Currency Currency, decimal Rate)
{
    public static CnbFxRow FromSeparatedString(string theString, char delimiter)
    {
        var split = theString.Split(delimiter);
        if (split.Length != 5)
        {
            throw new ArgumentException("Expected 5 parameters, got: " + split.Length);
        }
        
        Console.WriteLine(split[4]);
        // Decimal notation is a `,` comma, so parse it with a comma-supported culture/fmt, using FR here
        var parsedDecimal = decimal.Parse(split[4], NumberStyles.Number, new CultureInfo("fr-FR"));

        return new CnbFxRow(
            Country: split[0],
            CurrencyStr: split[1],
            Amount: long.Parse(split[2]),
            Currency: new Currency(split[3]),
            Rate: parsedDecimal
        );
    }
}
