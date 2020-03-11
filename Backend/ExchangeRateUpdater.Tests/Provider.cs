using System.Collections.Generic;
using System.Linq;
using Moq;
using Xunit;

namespace ExchangeRateUpdater
{
    public class Provider
    {
        private readonly ExchangeRateProvider provider;

        public Provider()
        {
            var rateProviderMock = new Mock<IRatesProvider>();
            rateProviderMock
                .Setup(a => a.GetAllRates())
                .Returns(new[]
                {
                    new ExchangeRate(new Currency("CZK"), new Currency("USD"), 1),
                    new ExchangeRate(new Currency("USD"), new Currency("CZK"), 2),
                    new ExchangeRate(new Currency("CZK"), new Currency("EUR"), 3),
                    new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 4),
                });

            this.provider = new ExchangeRateProvider(rateProviderMock.Object);
        }

        [Fact]
        public void NullCurrencies_EmptyRates()
        {
            //Arrange
            IEnumerable<Currency> currencies = null;

            //Act
            IEnumerable<ExchangeRate> rates = this.provider
                .GetExchangeRates(currencies);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void EmptyCurrencies_EmptyRates()
        {
            //Arrange
            IEnumerable<Currency> currencies = new Currency[0];

            //Act
            IEnumerable<ExchangeRate> rates = this.provider
                .GetExchangeRates(currencies);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void NonExistingCurrency_EmptyRates()
        {
            //Arrange
            var currencies = new Currency[] { new Currency("ABC"), };

            //Act
            IEnumerable<ExchangeRate> rates = this.provider
                .GetExchangeRates(currencies);

            //Assert
            Assert.Empty(rates);
        }

        [Fact]
        public void ExistingCurrency_SingleRate()
        {
            //Arrange
            var currencies = new Currency[] { new Currency("EUR"), };

            //Act
            IEnumerable<ExchangeRate> rates = this.provider
                .GetExchangeRates(currencies);

            //Assert
            Assert.Single(rates);
            Assert.Equal("CZK", rates.First().SourceCurrency.Code);
            Assert.Equal("EUR", rates.First().TargetCurrency.Code);
            Assert.Equal(3, rates.First().Value);
        }
    }
}