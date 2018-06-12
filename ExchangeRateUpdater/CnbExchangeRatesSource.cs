using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Implements IExchangeRatesSource
    /// Provides rates from Czech National Bank
    /// </summary>
    internal class CnbExchangeRatesSource : IExchangeRatesSource
    {
        private const string DefaultSourceCurrency = "CZK";
        private const int TargetCurrencyIndex = 3;
        private const int AmountIndex = 2;
        private const int RateIndex = 4;
        private const char ValuesSeparator = '|';
        private static readonly string[] EndOfLineArray = {"\r\n", "\n", "\r"};
        private readonly IWebClientWrapper _webClientWrapper;
        private readonly Uri _cnbRatesUri;

        public CnbExchangeRatesSource(IWebClientWrapper webClientWrapper, Uri cnbRatesUri)
        {
            _webClientWrapper = webClientWrapper;
            _cnbRatesUri = cnbRatesUri;
        }

        public IEnumerable<ExchangeRate> LoadExchangeRates()
        {
            var exchangeRatesString = _webClientWrapper.DownloadString(_cnbRatesUri);
            
            var lines = exchangeRatesString.Split(
                EndOfLineArray,
                StringSplitOptions.RemoveEmptyEntries
            ).Skip(2); // file header

            var exchangeRates = lines.Select(BuildExchangeRate);

            return exchangeRates;
        }
        
        private ExchangeRate BuildExchangeRate(string line)
        {
            try
            {
                var values = line.Split(ValuesSeparator);

                return new ExchangeRate(
                    new Currency(DefaultSourceCurrency),
                    new Currency((values[TargetCurrencyIndex])),
                    CalculateExchangeRateValue(values[AmountIndex], values[RateIndex]));
            }
            catch (Exception)
            {
                Console.WriteLine($"Error while reading cnb rates line '{line}'");
                throw;
            }
        }

        private decimal CalculateExchangeRateValue(string amount, string rate)
        {
            int amountValue = int.Parse(amount);
            decimal rateValue = decimal.Parse(rate);

            if (amountValue <= 0)
            {
                throw new InvalidOperationException($"Invalid Amount value: '{amountValue}'");
            }

            if (rateValue <= 0)
            {
                throw new InvalidOperationException($"Invalid Rate value: '{rateValue}'");
            }

            return rateValue / amountValue;
        }
    }
}