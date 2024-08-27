using ExchangeRateUpdater.Lib.Shared;
using ExchangeRateUpdater.Lib.v1CzechNationalBank.ExchangeRateProvider;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdate.CzechNationalBank.Test
{
    [TestFixture]
    public class CzechNationalBankExchangeRateProviderTests
    {
        private Mock<IExchangeRatesParallelHttpClient> _exchangeRatesParallelHttpClient;
        private IExchangeRateProviderSettings _settings;
        private CzechNationalBankExchangeRateProvider _sut;

        [SetUp]
        public void Setup()
        {
            _settings = new ExchangeRateProviderSettings(
                sourceUrl: "test",
                timeoutSeconds: 5,
                maxThreads: 1,
                rateLimitCount: 5,
                rateLimitDuration: 1,
                precision: 4
            );
            _exchangeRatesParallelHttpClient = new Mock<IExchangeRatesParallelHttpClient>();
            _sut = new CzechNationalBankExchangeRateProvider(
                _settings,
                _exchangeRatesParallelHttpClient.Object
            );
        }

        [TestCase("CZK", "USD", 20.0000, 0.0500)]
        [TestCase("CZK", "EUR", 25.0000, 0.0400)]
        public async Task GetExchangeRatesAsync_ReturnsCorrectExchangeRates(
            string sourceCode,
            string targetCode,
            decimal expectedRate,
            decimal expectedReversedRate
        )
        {
            // Arrange
            var currencies = new List<Currency>
            {
                new Currency(targetCode),
            };

            var mockExchangeValues = new List<ProviderExchangeRate>
            {
                new ProviderExchangeRate(new Currency("USD"), 0.05m, 1),
                new ProviderExchangeRate(new Currency("EUR"), 0.04m, 1),
                new ProviderExchangeRate(new Currency("JPY"), 0.0m, 1) // should be omitted due to zero value
            };

            _exchangeRatesParallelHttpClient
                .Setup(client => client.GetExchangeRatesAsync(It.IsAny<IEnumerable<Currency>>()))
                .ReturnsAsync(mockExchangeValues);

            // Act
            var result = _sut.GetExchangeRates(currencies);

            // Assert
            var rate = result.FirstOrDefault(r => r.SourceCurrency.Code == sourceCode && r.TargetCurrency.Code == targetCode);
            var reversedRate = result.FirstOrDefault(r => r.SourceCurrency.Code == targetCode && r.TargetCurrency.Code == sourceCode);

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.IsNotNull(rate);
            Assert.IsNotNull(reversedRate);
            Assert.AreEqual(expectedRate, Math.Round(rate.Value, 4));
            Assert.AreEqual(expectedReversedRate, Math.Round(reversedRate.Value, 4));
        }

        [Test]
        public void ProvidedCurrencyConversion_ShouldReturnCZK()
        {
            Assert.AreEqual("CZK", _sut.ProvidedCurrencyConversion.Code);
        }

        [Test]
        public void MapToExchangeRates_ShouldFilterOutZeroBaseRates()
        {
            var exchangeRates = new List<ProviderExchangeRate>
            {
                new ProviderExchangeRate(new Currency("GBP"), 0m, 0.03m)
            };

            var result = _sut.MapToExchangeRates(new Currency("CZK"), new List<Currency> { new Currency("GBP") }, exchangeRates);

            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public void MapToExchangeRates_ShouldFilterOutNullOrZeroMostRecentRates()
        {
            var exchangeRates = new List<ProviderExchangeRate>
            {
                new ProviderExchangeRate(new Currency("GBP"), -1,1),
                new ProviderExchangeRate(new Currency("JPY"), 0m,1)
            };

            var result = _sut.MapToExchangeRates(
                    new Currency("CZK"),
                    new List<Currency> {
                        new Currency("GBP"),
                        new Currency("JPY")
                    },
                    exchangeRates
                );

            Assert.That(result.Count(), Is.EqualTo(0));
        }

        [Test]
        public void CreateExchangeRate_ShouldReturnCorrectExchangeRates()
        {
            var sourceCurrency = new Currency("CZK");
            var targetCurrency = new Currency("USD");
            var exchangeRatesLookup = new Dictionary<Currency, ProviderExchangeRate>
            {
                { targetCurrency, new ProviderExchangeRate(targetCurrency, 23.215m, 1m) }
            };

            var result = _sut.CreateExchangeRate(sourceCurrency, targetCurrency, exchangeRatesLookup);

            var rate = result.FirstOrDefault(r => r.SourceCurrency.Code == "CZK" && r.TargetCurrency.Code == "USD");

            Assert.IsNotNull(rate);
            Assert.That(Math.Round(rate.Value, 4), Is.EqualTo(0.0431m));

            // Check the reversed rate
            var reversedRate = result.FirstOrDefault(r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK");

            Assert.IsNotNull(reversedRate);
            Assert.That(Math.Round(reversedRate.Value, 4), Is.EqualTo(23.215m));

        }
    }
}
