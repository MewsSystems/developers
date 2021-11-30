using System;
using System.Linq;
using ExchangeRateUpdater.BusinessLayer;
using ExchangeRateUpdater.Entities;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests
{
    public class CnbXlmRatesProviderTest
    {
        private CnbXlmRatesProvider provider;
        private TestCnbXmlSource testSource;

        [SetUp]
        public void Setup()
        {
            testSource = new TestCnbXmlSource();
            provider = new CnbXlmRatesProvider(testSource);
        }

        [Test]
        public void CheckNoCzkException()
        {
            Assert.Throws(typeof(ArgumentException),()=> provider.GetExchangeRates(Enumerable.Empty<Currency>()));
        }

        [Test]
        public void TestRates()
        {
            var targetCurrency = new Currency("CZK");
            var rates = new ExchangeRate[]
            {
                new ExchangeRate(new Currency("USD"), targetCurrency, 1.0m),
                new ExchangeRate(new Currency("EUR"), targetCurrency, 2.0m),
                new ExchangeRate(new Currency("RUR"), targetCurrency, 1.0m)
            };
            testSource.SetRates(rates,targetCurrency);
            var result = provider.
                GetExchangeRates(rates.Select(i => i.SourceCurrency).Append(targetCurrency));
            Assert.True(rates.All(i=>result.Any(r=>i.SourceCurrency == r.SourceCurrency&& i.Value == r.Value)));
        }
    }
}