using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateProviderTests
    {
        [Theory]
        [InlineData("ABC", 1)]
        [InlineData("DEF", 2)]
        [InlineData("GHA", 3)]
        public void GetAllExchangeRatesAsync_ReturnsCorrectExchangeRate(string requestedCurrency, decimal expectedValue)
        {
            var exchangeRateSource = A.Fake<IExchangeRateSource>();
            var provider = new ExchangeRateProvider(exchangeRateSource);

            A.CallTo(() => exchangeRateSource.GetAllExchangeRates()).Returns(new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("ABC"), new Currency("CZK"), 1),
                new ExchangeRate(new Currency("DEF"), new Currency("CZK"), 2),
                new ExchangeRate(new Currency("GHA"), new Currency("CZK"), 3)
            });

            var exchangeRates = provider.GetExchangeRates(new List<Currency>() { new Currency(requestedCurrency) });

            Assert.Single(exchangeRates);
            Assert.Equal(requestedCurrency, exchangeRates.Single().SourceCurrency.Code);
            Assert.Equal("CZK", exchangeRates.Single().TargetCurrency.Code);
            Assert.True(exchangeRates.Single().Value == expectedValue);
        }

        [Fact]
        public void GetAllExchangeRatesAsync_EmptyRequest_ReturnsEmptyAnswer()
        {
            var exchangeRateSource = A.Fake<IExchangeRateSource>();
            var provider = new ExchangeRateProvider(exchangeRateSource);

            A.CallTo(() => exchangeRateSource.GetAllExchangeRates()).Returns(new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("ABC"), new Currency("CZK"), 1),
                new ExchangeRate(new Currency("DEF"), new Currency("CZK"), 2),
                new ExchangeRate(new Currency("GHA"), new Currency("CZK"), 3)
            });

            var exchangeRates = provider.GetExchangeRates(new List<Currency>() { });

            Assert.Empty(exchangeRates);
        }

        [Fact]
        public void GetAllExchangeRatesAsync_UnknownCurrencies_AreIgnored()
        {
            var exchangeRateSource = A.Fake<IExchangeRateSource>();
            var provider = new ExchangeRateProvider(exchangeRateSource);

            A.CallTo(() => exchangeRateSource.GetAllExchangeRates()).Returns(new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("ABC"), new Currency("CZK"), 1),
                new ExchangeRate(new Currency("DEF"), new Currency("CZK"), 2),
                new ExchangeRate(new Currency("GHA"), new Currency("CZK"), 3)
            });

            var exchangeRates = provider.GetExchangeRates(new List<Currency>() 
            {
                new Currency("XYZ"),
                new Currency("ABC"),
                new Currency("QPR"),
            });

            Assert.Single(exchangeRates);
            Assert.Equal("ABC", exchangeRates.Single().SourceCurrency.Code);
            Assert.Equal("CZK", exchangeRates.Single().TargetCurrency.Code);
            Assert.True(exchangeRates.Single().Value == 1);
        }

        [Fact]
        public void GetAllExchangeRatesAsync_DuplicateRequestedCurrencies_ReturnsOneAnswer()
        {
            var exchangeRateSource = A.Fake<IExchangeRateSource>();
            var provider = new ExchangeRateProvider(exchangeRateSource);

            A.CallTo(() => exchangeRateSource.GetAllExchangeRates()).Returns(new List<ExchangeRate>()
            {
                new ExchangeRate(new Currency("ABC"), new Currency("CZK"), 1),
                new ExchangeRate(new Currency("DEF"), new Currency("CZK"), 2),
                new ExchangeRate(new Currency("GHA"), new Currency("CZK"), 3)
            });

            var exchangeRates = provider.GetExchangeRates(new List<Currency>()
            {
                new Currency("ABC"),
                new Currency("ABC"),
                new Currency("ABC"),
            });

            Assert.Single(exchangeRates);
            Assert.Equal("ABC", exchangeRates.Single().SourceCurrency.Code);
            Assert.Equal("CZK", exchangeRates.Single().TargetCurrency.Code);
            Assert.True(exchangeRates.Single().Value == 1);
        }

        [Fact]
        public void GetAllExchangeRatesAsync_EmptySource_ReturnsEmptyList()
        {
            var exchangeRateSource = A.Fake<IExchangeRateSource>();
            var provider = new ExchangeRateProvider(exchangeRateSource);

            A.CallTo(() => exchangeRateSource.GetAllExchangeRates()).Returns(new List<ExchangeRate>()
            {
                
            });

            var exchangeRates = provider.GetExchangeRates(new List<Currency>() { new Currency("DEF") });

            Assert.Empty(exchangeRates);
        }

    }
}
