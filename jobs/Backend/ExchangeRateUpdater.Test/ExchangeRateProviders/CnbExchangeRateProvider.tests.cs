using ExchangeRateUpdater.CoreClasses;
using ExchangeRateUpdater.ExchangeRateProviders;
using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using FakeItEasy;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.ExchangeRateProviders
{
    [TestFixture]
    public class CnbExchangeRateProvider_tests
    {
        IQuotesProvider quotesProvider;
        IQuotesParser quotesParser;
        CnbExchangeRateProvider rateProvider;
        Currency sourceCurrency;

        [SetUp]
        public void Init()
        {
            sourceCurrency = new Currency("RUB");

            Dictionary<Currency, ExchangeRate> ratesDic = new Dictionary<Currency, ExchangeRate>();
            ratesDic.Add(sourceCurrency, new ExchangeRate(sourceCurrency, 1, CnbExchangeRateProvider.BaseCurrency, 1, 99.999m));

            quotesProvider = new Fake<IQuotesProvider>().FakedObject;
            quotesParser = new Fake<IQuotesParser>().FakedObject;

            A.CallTo(() => quotesProvider.GetQuotesAsync()).Returns("fakedQoutes");
            A.CallTo(() => quotesParser.ParseQuotes(CnbExchangeRateProvider.BaseCurrency, "fakedQoutes")).Returns(ratesDic);

            rateProvider = new CnbExchangeRateProvider(quotesProvider, quotesParser);
        }

        [Test]
        public async Task GetExchangeRatesAsync_test()
        {
            var result = await rateProvider.GetExchangeRatesAsync(new[] { sourceCurrency });
            Assert.AreEqual(1, result.Count());
        }
    }
}
