using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using ExchangeRateUpdater.Lib.Shared;

namespace ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider
{
    /// <summary>
    /// utility class to handle provider exchange rate source data
    /// </summary>
    public class ProviderExchangeRate
    {
        const int DATE_OF_RATE = 0;
        const int EXCHANGE_RATE_VALUE = 1;

        public Currency Currency { get; private set; }
        public Dictionary<DateTime, decimal> Rates { get; private set; }
        public decimal BaseRate { get; private set; }

        public decimal? MostRecentRate
        {
            get
            {
                if (Rates == null || Rates.Count == 0) return null;
                var mostRecentDateIndex = Rates.Keys.Max();
                return Rates[mostRecentDateIndex];
            }
        }

        public ProviderExchangeRate()
        {

        }

        public ProviderExchangeRate(
            Currency currency,
            decimal rate,
            decimal baseMultiplier
        )
        {
            Currency = currency;
            BaseRate = baseMultiplier;

            // create an artifical rate
            Rates = new Dictionary<DateTime, decimal>
            {
                { DateTime.Today, rate }
            };
        }

        /// <summary>
        /// Deserialize Czech National Bank Exchange Rate Txt Format
        /// </summary>
        /// <param name="data"></param>
        /// <param name="precision"></param>
        /// <returns>
        /// ProviderExchangeRate
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static ProviderExchangeRate Deserialize(
            string data
            )
        {
            if (string.IsNullOrWhiteSpace(data))
                throw new ArgumentException("Invalid data format");

            // Extracting currency and amount
            string currencyPattern = @"Currency:\s*(?<currency>[A-Z]{3})\|Amount:\s*(?<amount>\d+)";
            var currencyMatch = Regex.Match(data, currencyPattern);

            if (!currencyMatch.Success)
                throw new ArgumentException("Invalid data format: Missing currency and amount");

            string currencyCode = currencyMatch.Groups["currency"].Value;
            decimal amount = decimal.Parse(currencyMatch.Groups["amount"].Value, CultureInfo.InvariantCulture);

            ProviderExchangeRate exchangeRate = new ProviderExchangeRate
            {
                Rates = new Dictionary<DateTime, decimal>(),
                Currency = new Currency(currencyCode),
                BaseRate = amount
            };

            // Split the exchange rate data lines
            string[] lines = data.Trim().Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 2; i < lines.Length; i++) // Start from index 2 (skipping the first two lines)
            {
                string[] parts = lines[i].Split('|');
                DateTime date = DateTime.ParseExact(parts[DATE_OF_RATE], "dd.MM.yyyy", CultureInfo.InvariantCulture);
                decimal rate = decimal.Parse(parts[EXCHANGE_RATE_VALUE], CultureInfo.InvariantCulture);
                exchangeRate.Rates.Add(date, rate);
            }

            return exchangeRate;
        }

    }

}