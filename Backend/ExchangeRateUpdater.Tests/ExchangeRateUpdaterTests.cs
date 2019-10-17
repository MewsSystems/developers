using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateUpdaterTests
    {
        private static IEnumerable<Currency> currencies = new[]
        {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
        };

        [Fact]
        public void CurrencyNotProvidedByCNBIgnored()
        {
            var exchangeRateProvider = new ExchangeRateProvider();
            var currencies = new List<Currency> { new Currency("BFK") };
            var exchangeRates = exchangeRateProvider.GetExchangeRates(currencies);

            Assert.Empty(exchangeRates);
        }

        [Fact]
        public void CountCurrencyRates()
        {
            var exchangeRateProvider = new ExchangeRateProvider();
            var exchangeRates = exchangeRateProvider.GetExchangeRates(currencies).ToList();

            Assert.Equal(2, exchangeRates.Count());
        }
    }
}
