using ExchangeRateUpdater.ExchangeRateStrategies.Cnb;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Strategies.Cnb
{
    [TestClass]
    public class CnbRatesHttpClientFetcherTests
    {
        [TestMethod]
        public void ShouldFetchDataWithoutFailing()
        {
            var url = "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";
            var fetcher = new CnbRatesHttpClientFetcher();

            var message = fetcher
                .FetchRatesAsync(url)
                .GetAwaiter()
                .GetResult();

            Assert.IsTrue(message.Length > 0);
        }
    }
}
