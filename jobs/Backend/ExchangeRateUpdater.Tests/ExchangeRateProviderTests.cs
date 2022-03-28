using NSubstitute;

using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Tests for <see cref="ExchangeRateProvider"/> class.
    /// </summary>
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private const string CZECH_CURRENCY_CODE = "CZK";

        private IExchangeRateCalculator calculator = null!;
        private ExchangeRateProvider provider = null!;

        [SetUp]
        public void SetUp()
        {
            calculator = Substitute.For<IExchangeRateCalculator>();

            provider = new ExchangeRateProvider(calculator);
        }

        [Test]
        public void CzechCrownDoesNotUseCalculator()
        {
            var currencies = new[] { new Currency(CZECH_CURRENCY_CODE) };

            var result = provider.GetExchangeRates(currencies);

            Assert.Multiple(() =>
            {
                var rate = result.Single();

                Assert.AreEqual(CZECH_CURRENCY_CODE, rate.SourceCurrency.Code);
                Assert.AreEqual(CZECH_CURRENCY_CODE, rate.TargetCurrency.Code);
                Assert.AreEqual(1.0M, rate.Value);
            });

            calculator.Received(0).TryGet(CZECH_CURRENCY_CODE, out _);
        }

        [Test]
        [TestCase(new object[] { "AUD" })]
        [TestCase(new object[] { "AUD", "USD", "EUR" })]
        public void CzechCrownDoesNotUseCalculator(params string[] currencyCodes)
        {
            var currencies = currencyCodes.Select(currencyCode => new Currency(currencyCode));

            var result = provider.GetExchangeRates(currencies);

            foreach (var currencyCode in currencyCodes)
            {
                calculator.Received(1).TryGet(currencyCode, out _);
            }
        }

        [Test]
        public void UnknownCurrencyDoesNotReturnExchangeRate()
        {
            const string UNKNOWN_CODE = "123";

            var currencies = new[] { new Currency(UNKNOWN_CODE) };

            var result = provider.GetExchangeRates(currencies);

            CollectionAssert.IsEmpty(result);

            calculator.Received(1).TryGet(UNKNOWN_CODE, out _);
        }
    }
}
