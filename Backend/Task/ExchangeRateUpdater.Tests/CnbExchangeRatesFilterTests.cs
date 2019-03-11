using NUnit.Framework;
using System.Linq;

namespace ExchangeRateUpdater.Tests
{
    [TestFixture]
    public class CnbExchangeRatesFilterTests
    {
        private readonly CnbExchangeRatesFilter _cnbExchangeRatesFilter = new CnbExchangeRatesFilter();

        [Test]
        public void GetFilteredRatesTest5ElementsEnumerable()
        {
            var czkCurrency = new Currency("CZK");
            var usdCurrency = new Currency("USD");
            var chfCurrency = new Currency("CHF");
            var dkkCurrency = new Currency("DKK");
            var unfilteredRates = new[]
            {
                new ExchangeRate(czkCurrency, czkCurrency, 1m),
                new ExchangeRate(new Currency("AUD"), czkCurrency, 15.989m),
                new ExchangeRate(dkkCurrency, czkCurrency, 3.432m),
                new ExchangeRate(chfCurrency, czkCurrency, 22.553m),
                new ExchangeRate(dkkCurrency, usdCurrency, 0.15m),
                new ExchangeRate(chfCurrency, usdCurrency, 0.99m),
            };
            var currencies = new[]
            {
                czkCurrency,
                chfCurrency,
                dkkCurrency,
            };
            var expectedResult = new[]
            {
                new ExchangeRate(czkCurrency, czkCurrency, 1m),
                new ExchangeRate(chfCurrency, czkCurrency, 22.553m),
                new ExchangeRate(chfCurrency, usdCurrency, 0.99m),
                new ExchangeRate(dkkCurrency, czkCurrency, 3.432m),
                new ExchangeRate(dkkCurrency, usdCurrency, 0.15m),
            };

            var actualResult = _cnbExchangeRatesFilter.GetFilteredRates(unfilteredRates, currencies);

            CollectionAssert.AllItemsAreInstancesOfType(actualResult, typeof(ExchangeRate));
            CollectionAssert.AreEqual(expectedResult, actualResult.ToArray(), new ExchangeRateComparer());
        }
    }
}
