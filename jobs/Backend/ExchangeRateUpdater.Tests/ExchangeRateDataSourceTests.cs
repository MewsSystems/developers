using ExchangeRateUpdater.DataSources;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateDataSourceTests
    {
        private const string ValidDailyRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        private const string ValidMonthlyRatesUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt";
        private const string InvalidUrl = "https://www.invalidurl.com/monthly.txt";

        private ExchangeRateDataSource dataSource;

        [SetUp]
        public void SetUp()
        {
            var options = new ExchangeRateDataSourceOptionsWrapper()
            {
                DailyRatesUrl = ValidDailyRatesUrl,
                MonthlyRatesUrl = ValidMonthlyRatesUrl
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            var httpClient = new HttpClient(new MockHttpMessageHandler());

            dataSource = new ExchangeRateDataSource(options, httpClient, cache);
        }


        [Test]
        public async Task GetExchangeRates_InvalidUrl_ReturnsEmptyList()
        {
            // Arrange
            var options = new ExchangeRateDataSourceOptionsWrapper()
            {
                DailyRatesUrl = InvalidUrl,
                MonthlyRatesUrl = InvalidUrl
            };

            var cache = new MemoryCache(new MemoryCacheOptions());
            var httpClient = new HttpClient(new MockHttpMessageHandler());

            var dataSource = new ExchangeRateDataSource(options, httpClient, cache);
            var currencies = new List<Currency>()
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // Act
            var result = await dataSource.GetExchangeRates(currencies);

            // Assert
            Assert.That(result, Is.Empty);
        }
    }

    public class MockHttpMessageHandler : HttpMessageHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> responses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            responses.Add(uri, responseMessage);
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
        {
            if (responses.ContainsKey(request.RequestUri))
            {
                return Task.FromResult(responses[request.RequestUri]);
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request });
        }
    }
}
