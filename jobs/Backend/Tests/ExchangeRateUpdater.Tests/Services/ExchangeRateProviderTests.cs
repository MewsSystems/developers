using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;

namespace ExchangeRateUpdater.Tests.Services
{
    public class ExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRatesAsync_ReturnsParsedRates_WhenHttpAndParserAreSuccessful()
        {
            var currencies = new List<Currency>
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            var fakeXml = "<kurzy><tabulka><radek kod=\"USD\" mnozstvi=\"1\" kurz=\"24,50\" /></tabulka></kurzy>";

            var parserMock = new Mock<ICnbXmlParser>();
            parserMock.Setup(p => p.Parse(It.IsAny<string>(), currencies)).Returns(new List<ExchangeRate>
            {
                new ExchangeRate(new Currency("CZK"), new Currency("USD"), 24.50m)
            });

            var loggerMock = new Mock<ILogger<ExchangeRateProvider>>();

            var options = Options.Create(new CnbOptions
            {
                DailyUrl = "https://example.com/fake"
            });

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<System.Threading.CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(fakeXml)
                });

            var httpClient = new HttpClient(handlerMock.Object);

            var provider = new ExchangeRateProvider(httpClient, loggerMock.Object, parserMock.Object, options);

            var result = await provider.GetExchangeRatesAsync(currencies);

            Assert.Single(result);
            Assert.Equal("USD", result.First().TargetCurrency.Code);
            Assert.Equal(24.50m, result.First().Value);
        }
    }
}
