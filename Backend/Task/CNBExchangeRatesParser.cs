using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class CNBExchangeRatesParser : IExchangeRatesParser
    {
        /// <summary>
        /// Parses the CNB text document containing FX rates to CZK.
        /// </summary>
        /// <param name="fxRatesRaw">The <see cref="string"/> to try to parse.</param>
        /// <returns>An Enumerable of the <see cref="ExchangeRate"/> class representing the list of parsed FX rates; the Enumerable is empty if no rates can be parsed.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="fxRatesRaw"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="fxRatesRaw"/> is empty.
        /// </exception>
        public IEnumerable<ExchangeRate> ParseExchangeRates(string fxRatesRaw)
        {

            if (fxRatesRaw == null) throw new ArgumentNullException(fxRatesRaw);
            if (fxRatesRaw.Trim().Equals(string.Empty)) throw new ArgumentException("The string to be parsed is empty!");

            var fxRatesList = new List<ExchangeRate>();
            var rateRegex = new Regex(@"([A-z]+\|[A-z]+\|\d+\|[A-Z]{3}\|\d+.\d+)"); // looking only for records of this format: 'Mexico|peso|1|MXN|1.209'

            var fxRatesStrList = fxRatesRaw.Split('\n').Where(rs => rateRegex.IsMatch(rs)); // I could have used rateRegex.Matches(fxRatesRaw) directly on the string
                                                                                            // but it felt like a better idea to have the string split by new line char first

            foreach (string rateStr in fxRatesStrList)
            {
                var rateVals = rateStr.Split('|');

                fxRatesList.Add(
                    new ExchangeRate(
                        new Currency(rateVals[3]), // the base currency
                        new Currency("CZK"), // The reason I am hard coding it here is that it's a concrete implementation of IExchangeRateParser specifically for CNB
                                             // and the bank provides only CZK for a secondary currency.
                        decimal.Parse(rateVals[4], CultureInfo.InvariantCulture) / decimal.Parse(rateVals[2]) // Using Parse here instead of TryPase because the Regex
                                                                                                              // already ensures "parsability" of the two values.
                    )
                );
            }

            return fxRatesList;
        }
    }
}
