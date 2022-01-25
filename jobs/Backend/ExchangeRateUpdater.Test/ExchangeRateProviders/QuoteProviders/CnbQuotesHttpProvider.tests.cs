using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using ExchangeRateUpdater.ExchangeRateProviders.QuotesProviders;
using NUnit.Framework;
using System;
using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.ExchangeRateProviders.QuoteProviders
{

    [TestFixture]
    public class CnbQuotesHttpProvider_tests
    {
        IQuotesProvider quotesProvider;
        IWebProxyProvider webProxyProvider;

        [SetUp]
        public void Init()
        {
            webProxyProvider = new Fake<IWebProxyProvider>().FakedObject;
            quotesProvider = new CnbQuotesHttpProvider(webProxyProvider);
            A.CallTo(() => webProxyProvider.GetUrlAsync(CnbQuotesHttpProvider.ProviderUrl)).Returns("faked");
        }

        [Test]
        public async Task GetUrlAsync_calledWithExpectedURL()
        {
            var result = await quotesProvider.GetQuotesAsync();
            Assert.AreEqual("faked", result);
            
        }
    }
}
