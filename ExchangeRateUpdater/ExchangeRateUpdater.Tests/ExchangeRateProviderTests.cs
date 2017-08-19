using FakeItEasy;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class ExchangeRateProviderTests
    {
        private IExchangeRatesSource source;
        private ExchangeRateProvider provider;

        [SetUp]
        public void SetUp()
        {
            source = A.Fake<IExchangeRatesSource>();
            provider = new ExchangeRateProvider(source);
        }
        [Test]
        public void AllExistingCurrenciesKnown_GetExchangeRatesWithNull_NoRateReturned()
        {
            AssertNoRates(null);
        }

        [Test]
        public void AllExistingCurrenciesKnown_GetExchangeRatesWithNoCurrency_NoRateReturned()
        {
            AssertNoRates(Enumerable.Empty<Currency>());
        }

        [Test]
        public void UnknownCurrency_GetExchangeRatesWithNoCurrency_NoRateReturned()
        {
            AssertNoRates(new List<Currency> { new Currency("XYZ") });
        }

        [Test]
        public void AllExistingCurrenciesKnown_GetExchangeRatesWithUnknownCurrency_NoRateReturned()
        {
            AssertNoRates(new List<Currency> { new Currency("XYZ") });
        }

        [Test]
        public void AllExistingCurrenciesKnown_GetExchangeRatesWithOneKnownCurrency_OneRateReturned()
        {
            var usd = new Currency("USD");
            A.CallTo(() => source.GetLatestRatesAsync(null, null)).WithAnyArguments()
                .Returns(Task.FromResult((IEnumerable<ExchangeRate>)new List<ExchangeRate> { new ExchangeRate(usd, usd, 1) }));

            var rates = provider.GetExchangeRates(new List<Currency> { usd });

            Assert.That(rates, Is.EquivalentTo(new List<ExchangeRate> { new ExchangeRate(usd, usd, 1) }));
        }

        [Test]
        public void AllExistingCurrenciesKnownAllRatesDefined_GetRatesWithTwoKnownCurrencies_TwoRatesReturned()
        {
            var usd = new Currency("USD");
            var eur = new Currency("EUR");
            A.CallTo(() => source.GetLatestRatesAsync(usd, A<IEnumerable<Currency>>._)).Returns(Task.FromResult((IEnumerable<ExchangeRate>)new List<ExchangeRate> {
                new ExchangeRate(usd, eur, 2),
                new ExchangeRate(usd, usd, 1)
            }));
            A.CallTo(() => source.GetLatestRatesAsync(eur, A<IEnumerable<Currency>>._)).Returns(Task.FromResult((IEnumerable<ExchangeRate>)new List<ExchangeRate> {
                new ExchangeRate(eur, usd, 0.5m) ,
                new ExchangeRate(eur, eur, 1) ,
            }));

            var rates = provider.GetExchangeRates(new List<Currency> { usd, eur });

            Assert.That(rates, Is.EquivalentTo(new List<ExchangeRate> {
                new ExchangeRate(usd, eur, 2),
                new ExchangeRate(eur, usd, 0.5m),
                new ExchangeRate(usd, usd, 1),
                new ExchangeRate(eur, eur, 1)
            }));
        }

        private void AssertNoRates(IEnumerable<Currency> currencies)
        {
            var rates = provider.GetExchangeRates(currencies);

            Assert.That(rates, Is.Empty);
        }
    }
}
