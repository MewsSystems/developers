using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProviderTest
    {
        private readonly ExchangeRateProvider _provider;

        private IEnumerable<Tuple<Currency, Currency>> _currencies;

        public ExchangeRateProviderTest()
        {
            _provider = new ExchangeRateProvider();
        }

        private void Setup(bool isValidData = true)
        {
            _currencies = new[]
            {
                new Tuple<Currency, Currency>(new Currency("EUR"), new Currency("USD")),
                new Tuple<Currency, Currency>(new Currency("EUR"), new Currency("JPY")),
                new Tuple<Currency, Currency>(new Currency("AUD"), new Currency("USD")),
                new Tuple<Currency, Currency>(new Currency("GBP"), new Currency("PLN")),
                isValidData ? new Tuple<Currency, Currency>(new Currency("CHF"), new Currency("EUR")) : new Tuple<Currency, Currency>(new Currency("ERU"), new Currency("USD"))
            };
        }

        [Fact]
        public void GetExchangeRates_ShouldReturnCertainResultCount()
        {
            Setup();

            var result = _provider.GetExchangeRates(_currencies);

            Assert.Equal(result.Count(), _currencies.Count());
        }
        
        [Fact]
        public void GetExchangeRates_ShouldThrowException_IfWrongExchangeProvided()
        {
            Setup(false);

            Assert.Throws<KeyNotFoundException>(() => _provider.GetExchangeRates(_currencies));
        }
    }
}
