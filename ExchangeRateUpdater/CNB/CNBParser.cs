using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CNB
{
    /// <summary>
    /// CNB parser
    /// </summary>
    public class CNBParser
    {

        /// <summary>
        /// The source currency of CNB
        /// </summary>
        private static Currency _sourceCurrency = new Currency("CZK");

        /// <summary>
        /// The CNB culture
        /// </summary>
        private static CultureInfo _CNBCultureInfo = new CultureInfo("cs-CZ");

        /// <summary>
        /// The target currencies (filter)
        /// </summary>
        private readonly IEnumerable<Currency> _targetCurrencies;

        /// <summary>
        /// Initializes a new instance of the <see cref="CNBParser"/> class.
        /// </summary>
        /// <param name="targetCurrencies">The target currencies.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public CNBParser(IEnumerable<Currency> targetCurrencies)
        {
            // validate parameters
            if (targetCurrencies == null || targetCurrencies.Count() == 0) throw new ArgumentNullException();

            _targetCurrencies = targetCurrencies;
        }

        /// <summary>
        /// Parses the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// Filtered exchange rates
        /// </returns>
        public async Task<IEnumerable<ExchangeRate>> Parse(Stream stream)
        {
            // result exchange rates
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            using (var reader = new StreamReader(stream))
            {
                // skip first two lines (headers)
                await reader.ReadLineAsync();
                await reader.ReadLineAsync();

                // Line processing may run in parallel, but data sets are too small to actually benefit from it.
                // If data sets become larger, we may test this option.
                while(!reader.EndOfStream)
                {
                    // get line as string
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // get exchange rate
                    var exchangeRate = ProcessLine(line);
                    if (exchangeRate == null) continue;

                    exchangeRates.Add(exchangeRate);
                }
            }

            return exchangeRates;
        }

        /// <summary>
        /// Processes the line.
        /// </summary>
        /// <param name="line">The line.</param>
        /// <returns>
        /// Exchange rate
        /// </returns>
        private ExchangeRate ProcessLine(string line)
        {
            // split to collumns and validate
            var collumns = line.Split('|');
            if (collumns.Count() != 5) return null;

            var targetCurrency = new Currency(collumns[3]);

            // value
            var valueParseResult = Decimal.TryParse(collumns[4], NumberStyles.Any, _CNBCultureInfo, out var value);

            // validation and filter
            if (!valueParseResult || !_targetCurrencies.Contains(targetCurrency))
                return null;

            var exchangeRate = new ExchangeRate(_sourceCurrency, targetCurrency, value);
            return exchangeRate;
        }
    }
}
