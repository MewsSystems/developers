using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.HttpClients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RichardSzalay.MockHttp;
using Xunit;

namespace ExchangeRateUpdaterTests.HttpClients
{
    public class ExchangeRateFetcherTests
    {
        [Fact]
        public async Task FetchAsync_ReturnsExpectedRawText()
        {
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When("https://mock-daily")
                    .Respond("text/plain", "Mock CNB Data");
            var mockLogger = new Mock<ILogger<DailyExchangeRateFetcher>>();

            var httpClient = mockHttp.ToHttpClient();

            var fetcher = new DailyExchangeRateFetcher(
                httpClient,
                Options.Create(new CzechBankSettings { DailyRatesUrl = "https://mock-daily" }),
                mockLogger.Object
            );

            var result = await fetcher.FetchAsync();
            Assert.Equal("Mock CNB Data", result);
        }
    }
}
