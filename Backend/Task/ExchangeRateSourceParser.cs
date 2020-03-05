using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    /// <summary>
    ///     Parser class for exchange rate source parsing. Learn more about source structure at
    ///     https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/.
    /// </summary>
    public class ExchangeRateSourceParser
    {
        private const int CurrencyVolumeIndex = 2;
        private const int CurrencyCodeIndex = 3;
        private const int ExchangeRateIndex = 4;
        private const int HeaderOffset = 2;
        private const char Delimiter = '|';
        
        private readonly NumberFormatInfo _formatter;
        private readonly Currency _targetCurrency;

        /// <summary>
        ///     Initializes new instance of the <see cref="ExchangeRateSourceParser"/> with target currency.
        /// </summary>
        /// <param name="targetCurrency">The target currency.</param>
        public ExchangeRateSourceParser(Currency targetCurrency)
        {
            _targetCurrency = targetCurrency ?? throw new ArgumentNullException(nameof(targetCurrency));
            _formatter = new NumberFormatInfo
            {
                NumberDecimalSeparator = ","
            };
        }
        
        /// <summary>
        ///     Parses exchange rates from source in csv format.
        /// </summary>
        /// <param name="source">The source string.</param>
        /// <returns>The parsed exchange rates.</returns>
        public async Task<IEnumerable<ExchangeRate>> ParseExchangeRatesAsync(string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return Enumerable.Empty<ExchangeRate>();
            
            var exchangeRates = new List<ExchangeRate>();
            using (var reader = new StringReader(source))
            {
                var currentLine = 0;
                while (true)
                {
                    var line = await reader.ReadLineAsync();
                    if (string.IsNullOrWhiteSpace(line))
                        break;

                    currentLine++;
                    if (currentLine <= HeaderOffset)
                        continue;
                
                    exchangeRates.Add(ParseLine(line));
                }

                return exchangeRates;
            }
        }
        
        private ExchangeRate ParseLine(string line)
        {
            var lineParts = line.Split(Delimiter);
            if (lineParts.Length < ExchangeRateIndex + 1)
                throw new FormatException("Invalid line format of exchange rate source.");
            
            var currencyVolume = ParseCurrencyVolume(lineParts);
            var exchangeRate = ParseExchangeRate(lineParts, _formatter);
            
            var currencyCode = lineParts[CurrencyCodeIndex];
            var sourceCurrency = new Currency(currencyCode);
            
            return new ExchangeRate(sourceCurrency, _targetCurrency, exchangeRate / currencyVolume);
        }

        private static int ParseCurrencyVolume(IReadOnlyList<string> lineParts)
        {
            if (!int.TryParse(lineParts[CurrencyVolumeIndex], out var volume))
                throw new FormatException("Invalid currency volume of exchange rate source.");

            return volume;
        }
        
        private static decimal ParseExchangeRate(IReadOnlyList<string> lineParts, IFormatProvider formatter)
        {
            if (!decimal.TryParse(lineParts[ExchangeRateIndex], NumberStyles.AllowDecimalPoint, formatter, out var rate))
                throw new FormatException("Invalid rate of exchange rate source.");

            return rate;
        }
    }
}