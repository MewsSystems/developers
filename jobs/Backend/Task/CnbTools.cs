using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace ExchangeRateUpdater
{
    public static class CnbTools
    {
        /// <summary>
        /// For a given text of CNB exchange rate file returns parsed exchange rates with respect to source currency.
        /// <para />
        /// Format of a CNB currency file should be CSV file with date row, header row and '|' as separator.
        /// Columns are: Country|Currency|Amount|Code|Rate
        /// </summary>
        /// <param name="sourceCurrency"></param>
        /// <param name="exchangeRateText"></param>
        /// <returns></returns>
        public static IEnumerable<ExchangeRate> ParseExchangeRates(Currency sourceCurrency, string exchangeRateText)
        {
            ArgumentNullException.ThrowIfNull(sourceCurrency, nameof(sourceCurrency));
            ArgumentNullException.ThrowIfNull(exchangeRateText, nameof(exchangeRateText));

            using var reader = new StringReader(exchangeRateText);

            reader.ReadLine();
            reader.ReadLine();

            string? line = null;
            while ((line = reader.ReadLine()) != null)
            {
                var columns = line.Split('|');
                if (columns.Length != 5)
                    continue; // invalid or comment line

                if (!int.TryParse(columns[2], out var amount)
                    || !decimal.TryParse(columns[4], NumberStyles.Any, CultureInfo.InvariantCulture, out var rate))
                    continue; // line with invalid values

                var targetCurrency = new Currency(columns[3]);
                yield return new ExchangeRate(sourceCurrency, targetCurrency, rate / amount);
            }
        }
    }
}