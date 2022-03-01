using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Providers.Cnb;

/// <summary>
/// CNB Exchange rate CSV parser
/// </summary>
public class CnbCsvParser
{
    /// <summary>
    /// Describe number format used for Rate column in CVS
    /// </summary>
    private readonly NumberFormatInfo _numberFormatProvider;
    
    /// <summary>
    /// Target currency is CZK because CNB only offers rates to CZK
    /// </summary>
    private readonly Currency _targetCurrency = new("CZK");
    
    /// <summary>
    /// Describe CSV
    /// </summary>
    private static class CNB_CSV_COLUMN_DESCRIPTOR
    {
        public static string ColumnSeparator => "|";

        public static class Clomuns
        {
            public static int Country => 0;
            public static int Currency => 1;
            public static int Amount => 2;
            public static int Code => 3;
            public static int Rate => 4;
        }
    }
    
    public CnbCsvParser()
    {
        _numberFormatProvider = new NumberFormatInfo()
        {
            CurrencyDecimalSeparator = ".",
            NumberGroupSeparator = ""
        };
    }

    /// <summary>
    /// Parse CNB Exchange rates csf file, line by line
    /// </summary>
    /// <param name="csvDataStreamm">CSV data stream</param>
    /// <returns>Parsed rates</returns>
    public IEnumerable<ExchangeRate> ParseExchangeRates(Stream csvDataStreamm)
    {
        using (StreamReader readStream = new StreamReader(csvDataStreamm))
        {
            readStream.ReadLine(); // Skip date info
            readStream.ReadLine(); // Skip header

            string line;
            while ((line = readStream.ReadLine()) != null)
            {
                ExchangeRate exchangeRate = ParseLine(line);
                if (exchangeRate != null)
                {
                    yield return exchangeRate;
                }
            }
        }
    }

    /// <summary>
    /// Parse CNB Exchange csv line to <see cref="ExchangeRate"/>
    /// </summary>
    /// <param name="line">CSV Line</param>
    /// <returns>Parsed <see cref="ExchangeRate"/> or Null</returns>
    private ExchangeRate ParseLine(string line)
    {
        var columns = line.Split(CNB_CSV_COLUMN_DESCRIPTOR.ColumnSeparator);
        var rateParsaSucceed = Decimal.TryParse(columns[CNB_CSV_COLUMN_DESCRIPTOR.Clomuns.Rate],
            NumberStyles.Number | NumberStyles.AllowDecimalPoint,
            _numberFormatProvider,
            out decimal rate);

        if (!rateParsaSucceed)
        {
            return null;
        }

        Currency sourceCurrency = new Currency(columns[CNB_CSV_COLUMN_DESCRIPTOR.Clomuns.Code]);

        var exchnageRate = ExchangeRate.Create(sourceCurrency, _targetCurrency, rate);
        return exchnageRate;
    }
}