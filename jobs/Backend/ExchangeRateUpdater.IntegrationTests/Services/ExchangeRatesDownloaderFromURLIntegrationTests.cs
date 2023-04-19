using ExchangeRateUpdater.WebApi.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ExchangeRateUpdater.IntegrationTests.Controllers
{
    public class ExchangeRatesDownloaderFromURLIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        public const string CorrectUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";
        public const string WrongUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/monthly.txt";
        public const string ExpectedWrongURLMessage = $"Request to URL '{WrongUrl}' did not work";

        [Fact]
        public async Task ExchangeRatesDownloaderFromURL_ShouldReturnRawText_WhenURLIsCorrect_IfURLExists()
        {
            using (var client = new HttpClient())
            {
                var exchangeRatesDownloaderFromURL = new ExchangeRatesDownloaderFromURL(client);
                var textResponse = await exchangeRatesDownloaderFromURL.GetExchangeRatesRawTextFromURL(CorrectUrl);

                Assert.NotEmpty(textResponse);
            }
        }

        [Fact]
        public async Task ExchangeRatesDownloaderFromURL_ShouldReturnSpecificExceptionMessage_WhenURLIsWrong()
        {
            using (var client = new HttpClient())
            {
                var exchangeRatesDownloaderFromURL = new ExchangeRatesDownloaderFromURL(client);

                Exception exception = await Assert.ThrowsAsync<Exception>(() => exchangeRatesDownloaderFromURL.GetExchangeRatesRawTextFromURL(WrongUrl));
                Assert.Equal(ExpectedWrongURLMessage, exception.Message);
            }
        }
    }
}