using System.Globalization;
using ExchangeRateUpdater.Clients.Cnb.Models;

namespace ExchangeRateUpdater.Clients.Cnb.Parsers;

public class CnbClientResponseParser
{
    private const char LineDelimiter = '|';
    private const string CurrencyDecimalSeparator = ".";
    private const int LineLength = 5;

    public ExchangeRate? ExtractExchangeRate(string line)
    {
        ExchangeRate? exchangeRate = null;

        var splintedLine = line.Split(LineDelimiter);

        if (splintedLine.Length != LineLength) return exchangeRate;

        try
        {
            exchangeRate = new ExchangeRate
            {
                Country = splintedLine[0],
                Currency = splintedLine[1],
                Amount = Convert.ToInt32(splintedLine[2]),
                Code = splintedLine[3],
                Rate = Convert.ToDecimal(splintedLine[4],
                    new NumberFormatInfo { CurrencyDecimalSeparator = CurrencyDecimalSeparator })
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return exchangeRate;
    }
}