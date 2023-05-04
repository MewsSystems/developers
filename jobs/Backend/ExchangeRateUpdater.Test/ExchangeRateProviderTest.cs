using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Test
{
    public class ExchangeRateProviderTest
    {
        [Test]
        public void TestGetExchangeRatesWhenNoCurrenciesAreRequired() {
            IReadOnlyList<ExchangeRate> setupExchangeRates = new[] { 
                CreateExchangeRate("CZK", "USD", 20m),
                CreateExchangeRate("CZK", "JPY", 10m),
                CreateExchangeRate("CZK", "NOK", 0.5m) };
            var exchangeRatesLinstingCacheMock = new Mock<IExchangeRatesListingsCache>(MockBehavior.Strict);
            exchangeRatesLinstingCacheMock.Setup(x => x.GetCurrentExchangeRates()).Returns(Task.FromResult(setupExchangeRates));

            var testObj = new ExchangeRateProvider(exchangeRatesLinstingCacheMock.Object);
                        
            var result = testObj.GetExchangeRates(Array.Empty<Currency>()).Result;

            Assert.IsEmpty(result);
            exchangeRatesLinstingCacheMock.Verify(x => x.GetCurrentExchangeRates(), Times.Once);
        }

        [Test]
        public void TestGetExchangeRatesWhenNoExchangeRatesAreAvailable() {
            IReadOnlyList<ExchangeRate> setupExchangeRates = Array.Empty<ExchangeRate>();
            var exchangeRatesLinstingCacheMock = new Mock<IExchangeRatesListingsCache>(MockBehavior.Strict);
            exchangeRatesLinstingCacheMock.Setup(x => x.GetCurrentExchangeRates()).Returns(Task.FromResult(setupExchangeRates));

            var testObj = new ExchangeRateProvider(exchangeRatesLinstingCacheMock.Object);

            var requestedCurrencies = new[] { new Currency("CZK"), new Currency("NOK"), new Currency("JPY") };
            var result = testObj.GetExchangeRates(requestedCurrencies).Result;

            Assert.IsEmpty(result);
            exchangeRatesLinstingCacheMock.Verify(x => x.GetCurrentExchangeRates(), Times.Once);
        }

        [Test]
        public void TestGetExchangeRatesWhenNoExchangeRatesForRequestedCurrecenciesAreAvailable() {
            IReadOnlyList<ExchangeRate> setupExchangeRates = new[] {
                CreateExchangeRate("CZK", "USD", 20m),
                CreateExchangeRate("CZK", "JPY", 10m),
                CreateExchangeRate("CZK", "NOK", 0.5m) };
            var exchangeRatesLinstingCacheMock = new Mock<IExchangeRatesListingsCache>(MockBehavior.Strict);
            exchangeRatesLinstingCacheMock.Setup(x => x.GetCurrentExchangeRates()).Returns(Task.FromResult(setupExchangeRates));

            var testObj = new ExchangeRateProvider(exchangeRatesLinstingCacheMock.Object);

            var requestedCurrencies = new[] { new Currency("MXN"), new Currency("GBP"), new Currency("PHP") };
            var result = testObj.GetExchangeRates(requestedCurrencies).Result;

            Assert.IsEmpty(result);
            exchangeRatesLinstingCacheMock.Verify(x => x.GetCurrentExchangeRates(), Times.Once);
        }

        [Test]
        public void TestGetExchangeRatesWhenSomeExchangeRatesForRequestedCurrecenciesAreAvailable() {
            var czkUsdExchangeRate = CreateExchangeRate("CZK", "USD", 20m);
            var usdCzkExchangeRate = CreateExchangeRate("USD", "CZK", 0.05m);
            var nokCzkExchangeRate = CreateExchangeRate("NOK", "CZK", 30m);
            var eurNokExchangeRate = CreateExchangeRate("EUR", "NOK", 0.5m);

            IReadOnlyList<ExchangeRate> setupExchangeRates = new[] {
                czkUsdExchangeRate,
                usdCzkExchangeRate,
                CreateExchangeRate("CZK", "JPY", 10m),
                nokCzkExchangeRate,
                eurNokExchangeRate};
            var exchangeRatesLinstingCacheMock = new Mock<IExchangeRatesListingsCache>(MockBehavior.Strict);
            exchangeRatesLinstingCacheMock.Setup(x => x.GetCurrentExchangeRates()).Returns(Task.FromResult(setupExchangeRates));

            var testObj = new ExchangeRateProvider(exchangeRatesLinstingCacheMock.Object);

            var requestedCurrencies = new[] { new Currency("USD"), new Currency("CZK"), new Currency("EUR") , new Currency("NOK")};
            var result = testObj.GetExchangeRates(requestedCurrencies).Result;

            Assert.AreEqual(4, result.Count());
            AssertExpectedExchangeRate(czkUsdExchangeRate, result.ElementAt(0));
            AssertExpectedExchangeRate(usdCzkExchangeRate, result.ElementAt(1));
            AssertExpectedExchangeRate(nokCzkExchangeRate, result.ElementAt(2));
            AssertExpectedExchangeRate(eurNokExchangeRate, result.ElementAt(3));

            exchangeRatesLinstingCacheMock.Verify(x => x.GetCurrentExchangeRates(), Times.Once);
        }

        private ExchangeRate CreateExchangeRate(string sourceCurrencyCode, string targetCurrencyCode, decimal value) {
            return new ExchangeRate(
                new Currency(sourceCurrencyCode), new Currency(targetCurrencyCode), value);
        }

        private void AssertExpectedExchangeRate(ExchangeRate expectedExchangeRate, ExchangeRate actualExchangeRate) {
            Assert.AreEqual(expectedExchangeRate.SourceCurrency, actualExchangeRate.SourceCurrency);
            Assert.AreEqual(expectedExchangeRate.TargetCurrency, actualExchangeRate.TargetCurrency);
            Assert.AreEqual(expectedExchangeRate.Value, actualExchangeRate.Value);
        }
    }
}
