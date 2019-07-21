using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
    public class RateSourceParcer : IRateSourceParcer
    {
        private const string CzkIsoCode = "CZK";
        private const int lineToStartParceSource = 2;

        /// <summary>
        /// Parces Currency Exchange rate source to the list of exchange rates
        /// </summary>
        /// <param name="rateSource">Content of the input file</param>
        /// <returns>IEnumerable of the actual exchange rates</returns>
        public IEnumerable<ExchangeRate> ParceRateSource(IEnumerable<string> rateSource)
        {
            var result = new List<ExchangeRate>();
            foreach (var source in rateSource)
            {
                string[] lines = source.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for (int i = lineToStartParceSource; i < lines.Length; i++)
                {
                    string[] items = lines[i].Split('|');

                    if (items.Length != 5)
                    {
                        throw new FormatException($"Unexpected format of parced source: {source}");
                    }

                    string sourceCurrency = items[3]?.Trim()?.ToUpper();
                    if (string.IsNullOrEmpty(sourceCurrency))
                    {
                        throw new FormatException($"Error parsing currency code from the source: {source}");
                    }

                    string rateString = items[4]?.Trim();
                    if (string.IsNullOrEmpty(rateString))
                    {
                        throw new FormatException($"Error parsing rate from the source: {source}");
                    }

                    string amountString = items[2]?.Trim();
                    if (string.IsNullOrEmpty(amountString))
                    {
                        throw new FormatException($"Error parsing amount from the source: {source}");
                    }

                    if (!decimal.TryParse(rateString, out decimal rate))
                    {
                        throw new FormatException($"Error converting rate from the string: {rateString}");
                    }

                    if (!int.TryParse(amountString, out int amount))
                    {
                        throw new FormatException($"Error converting amount from the string: {amountString}");
                    }

                    result.Add(new ExchangeRate(
                        new Currency(sourceCurrency), 
                        new Currency(CzkIsoCode), 
                        rate/amount));
                }
            }

            return result;
        }
    }
}
