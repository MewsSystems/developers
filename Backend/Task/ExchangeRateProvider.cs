using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ExchangeRateUpdater.ServiceContracts;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Implementation of <see cref="IExchangeRateProvider"/>.
    /// </summary>
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private const string FieldSeparator = "|"; // Field delimiter in raw data
        private const string DecimalDelimiter = ","; // Decimal delimiter in raw data
        private const string GroupDelimiter = "."; // Group delimiter in raw data
        private const string CZKCode = "CZK"; // Czech koruna name in ISO format.
        private const int SkipLineCount = 2; // Number of ignored lines in raw data
        private const int AmountIndex = 2; // Amount of a currency to be exchanged
        private const int CurrencyIndex = 3; // ISO code of a currency
        private const int ExchangeIndex = 4; // Exchange rate of a currency against CZK

        private readonly IExchangeRateService exchangeRateService = null;
        private readonly CultureInfo parseCulture = null;

        public ExchangeRateProvider()
        {
            exchangeRateService = new ExchangeRateService();

            parseCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            parseCulture.NumberFormat.NumberDecimalSeparator = DecimalDelimiter;
            parseCulture.NumberFormat.NumberGroupSeparator = GroupDelimiter;
        }

        /// <inheritdoc />
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var request = new LoadExchangeRatesRequest()
            {
                ExchangeDate = string.Empty,
                UseCache = false
            };

            var response = exchangeRateService.LoadExchangeRates(request);
            return ProcessExchangeRateResponse(response, currencies);
        }

        /// <summary>
        /// Processes a load exchange response and filter data by a given list of currencies in the provider.
        /// </summary>
        /// <param name="exchangeRatesResponse">The load exchange response.</param>
        /// <param name="proceedCurrencies">The filter.</param>
        /// <returns>Exchange rates among the specified currencies against CZK.</returns>
        private IEnumerable<ExchangeRate> ProcessExchangeRateResponse(LoadExchangeRatesResponse exchangeRatesResponse, IEnumerable<Currency> proceedCurrencies)
        {
            List<ExchangeRate> result = new List<ExchangeRate>();
            var targetCurrency = new Currency(CZKCode);

            var responseLines = exchangeRatesResponse.Lines.Skip(SkipLineCount);
            foreach (string line in responseLines)
            {
                var fields = line.Split(new[] {FieldSeparator}, StringSplitOptions.None);
                string currency = fields[CurrencyIndex];

                if (!proceedCurrencies.Any(x => x.Code.Equals(currency, StringComparison.InvariantCultureIgnoreCase)))
                {
                    continue;
                }

                int amount = Convert.ToInt32(fields[AmountIndex]);
                decimal rate = decimal.Parse(fields[ExchangeIndex], parseCulture);

                var sourceCurrency = new Currency(currency);
                result.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate/amount));
            }

            return result;
        }
    }
}
