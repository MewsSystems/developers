using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// This is an integration test class, because it depends on external service.
    /// </summary>
    public class CnbClientIntegrationTest
    {
        private readonly CnbClient _cnbClient;

        public CnbClientIntegrationTest()
        {
            _cnbClient = new CnbClient(new HttpClient());
        }

        [Fact]
        public async Task ReadExchangeRatesFromUrl_IncorrectUrl_Empty()
        {
            var result = await _cnbClient.ReadExchangeRatesFromUrlAsync("incorrect url");

            Assert.Empty(result);
        }

        [Fact]
        public async Task ReadExchangeRatesFromUrl_CorrectMajorUrl_NotEmpty()
        {
            var result = await _cnbClient.ReadExchangeRatesFromUrlAsync("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?date=10.12.2019");

            Assert.NotEmpty(result);
            Assert.Contains(result, x => x.CurrencyCode == "AUD" && x.ExchangeRateNormalized == 15.683M);
        }

        [Fact]
        public async Task ReadExchangeRatesFromUrl_CorrectMinorUrl_NotEmpty()
        {
            var result = await _cnbClient.ReadExchangeRatesFromUrlAsync("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.xml?date=29.11.2019");

            Assert.NotEmpty(result);
            Assert.Contains(result, x => x.CurrencyCode == "AFN" && x.ExchangeRateNormalized == 0.29748M);
        }
    }
}
