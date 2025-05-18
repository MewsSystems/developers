using System;
using System.Collections.Generic;
using System.Linq;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers
{
    /// <summary>
    /// Provides functionality to parse raw data into a structured collection of <see cref="ExchangeRate"/> objects.
    /// </summary>
    public class TextParser : IParser
    {
        private readonly Currency _targetCurrency;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextParser"/> class
        /// using "CZK" as a target currency.
        /// </summary>
        public TextParser()
        {
            _targetCurrency = new Currency("CZK");
        }

        /// <summary>
        /// Extracts exchange rate information from the input string.
        /// </summary>
        /// <param name="input">The raw exchange rate data string to parse.</param>
        /// <returns>An <see cref="IEnumerable{ExchangeRate}"/> extracted from the raw data.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="input"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <paramref name="input"/> is empty.</exception>
        public IEnumerable<ExchangeRate> ParseData(string input)
        {
            if (input is null)
            {
                throw new ArgumentNullException(nameof(input), "Input cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException("Input cannot be empty or whitespace.", nameof(input));
            }

            var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Skip(2);
            List<ExchangeRate> exchangeRates = new List<ExchangeRate>();

            foreach (var line in lines)
            {
                var lineContent = line.Split('|');

                var sourceCurrency = new Currency(lineContent[3]);
                var normalizedRate = NormalizeRate(decimal.Parse(lineContent[4]), int.Parse(lineContent[2]));

                exchangeRates.Add(new ExchangeRate(sourceCurrency, _targetCurrency, normalizedRate));
            }

            return exchangeRates;
        }

        /// <summary>
        /// Calculates normalized exchange rate by dividng the raw rate by amount of currency units.
        /// </summary>
        /// <param name="rate">The raw exchange rate value.</param>
        /// <param name="amount">Number of currency units.</param>
        /// <returns>Normalized exchange rate (price per currency unit).</returns>
        private static decimal NormalizeRate(decimal rate, int amount) => rate / amount;
    }
}