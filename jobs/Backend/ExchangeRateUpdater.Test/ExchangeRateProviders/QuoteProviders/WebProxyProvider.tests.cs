using ExchangeRateUpdater.ExchangeRateProviders.Interfaces;
using ExchangeRateUpdater.ExchangeRateProviders.QuotesProviders;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Test.ExchangeRateProviders.QuoteProviders
{
    [TestFixture]
    public class WebProxyProvider_tests
    {
        IWebProxyProvider webProxyProvider;
        [SetUp]
        public void Init()
        {
            webProxyProvider = new WebProxyProvider();
        }

        [Test]
        public void GetUrlAsync_Exceptions_tests()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async() => await webProxyProvider.GetUrlAsync(null));
            Assert.ThrowsAsync<ArgumentException>(async () => await webProxyProvider.GetUrlAsync(" "));
        }
    }
}
