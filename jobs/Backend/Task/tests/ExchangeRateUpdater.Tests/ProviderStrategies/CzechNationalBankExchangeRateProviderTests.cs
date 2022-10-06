using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Providers.ProvidersStrategies;
using ExchangeRateUpdater.Providers.Tests.Fakes;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater.Providers.Tests.ProviderStrategies
{
    public class CzechNationalBankExchangeRateProviderTests : MoqMeUp<CzechNationalBankExchangeRateProvider>
    {
        private const string ExchangeRateResponseMock = "05 Oct 2022 #193\nCountry|Currency|Amount|Code|Rate\nAustralia|dollar|1|AUD|15.954\nBrazil|real|1|BRL|4.761\nBulgaria|lev|1|BGN|12.541\nCanada|dollar|1|CAD|18.185\nChina|renminbi|1|CNY|3.480\nCroatia|kuna|1|HRK|3.260\nDenmark|krone|1|DKK|3.297\nEMU|euro|1|EUR|24.525\nHongkong|dollar|1|HKD|3.153\nHungary|forint|100|HUF|5.789\nIceland|krona|100|ISK|17.357\nIMF|SDR|1|XDR|31.885\nIndia|rupee|100|INR|30.350\nIndonesia|rupiah|1000|IDR|1.629\nIsrael|new shekel|1|ILS|6.989\nJapan|yen|100|JPY|17.134\nMalaysia|ringgit|1|MYR|5.346\nMexico|peso|1|MXN|1.236\nNew Zealand|dollar|1|NZD|14.089\nNorway|krone|1|NOK|2.338\nPhilippines|peso|100|PHP|42.158\nPoland|zloty|1|PLN|5.119\nRomania|leu|1|RON|4.965\nSingapore|dollar|1|SGD|17.377\nSouth Africa|rand|1|ZAR|1.393\nSouth Korea|won|100|KRW|1.745\nSweden|krona|1|SEK|2.263\nSwitzerland|franc|1|CHF|25.131\nThailand|baht|100|THB|66.127\nTurkey|lira|1|TRY|1.332\nUnited Kingdom|pound|1|GBP|28.083\nUSA|dollar|1|USD|24.754";

        [Fact]
        public async Task GetExchangeRates_DefaultBehaviour_ShouldReturnRequestedCurrencies()
        {
            // Arrange
            var fakeHttpMessageHandler = new HttpMessageHandlerFake(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(ExchangeRateResponseMock, Encoding.UTF8, "application/json")
            });

            var httpClientFactoryMock = this.Get<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler));

            // Act
            var target = this.Build();
            var act = await target.GetExchangeRates(
                new List<Currency>
                {
                    new Currency("EUR"),
                    new Currency("BRL")
                });

            // Assert
            act.Should().NotBeNullOrEmpty()
                .And.HaveCount(2)
                .And.Contain(x => x.SourceCurrency.Code == "EUR")
                .And.Contain(x => x.SourceCurrency.Code == "BRL");
        }

        [Fact]
        public async Task GetExchangeRates_WhenCzechBankReturnsBadRequest_ShouldReturnEmptyList()
        {
            // Arrange
            var fakeHttpMessageHandler = new HttpMessageHandlerFake(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("Something went wrong!", Encoding.UTF8, "application/json")
            });

            var httpClientFactoryMock = this.Get<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler));

            // Act
            var target = this.Build();
            var act = await target.GetExchangeRates(
                new List<Currency>
                {
                    new Currency("EUR")
                });

            // Assert
            act.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRates_WhenCzechBankReturnsEmptyResponse_ShouldReturnEmptyList()
        {
            // Arrange
            var fakeHttpMessageHandler = new HttpMessageHandlerFake(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Empty, Encoding.UTF8, "application/json")
            });

            var httpClientFactoryMock = this.Get<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler));

            // Act
            var target = this.Build();
            var act = await target.GetExchangeRates(
                new List<Currency>
                {
                    new Currency("EUR")
                });

            // Assert
            act.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRates_WhenCzechBankResponseDoesNotReturnAnyCurrency_ShouldReturnEmptyList()
        {
            // Arrange
            var responseWithZeroCurrencies = "05 Oct 2022 #193\nCountry|Currency|Amount|Code|Rate";

            var fakeHttpMessageHandler = new HttpMessageHandlerFake(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(responseWithZeroCurrencies, Encoding.UTF8, "application/json")
            });

            var httpClientFactoryMock = this.Get<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler));

            // Act
            var target = this.Build();
            var act = await target.GetExchangeRates(
                new List<Currency>
                {
                    new Currency("EUR")
                });

            // Assert
            act.Should().BeEmpty();
        }

        [Fact]
        public async Task GetExchangeRates_WhenCurrencyThatDoesNotExistsIsRequested_ShouldOnlyReturnCurrenciesThatExists()
        {
            // Arrange
            var fakeHttpMessageHandler = new HttpMessageHandlerFake(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(ExchangeRateResponseMock, Encoding.UTF8, "application/json")
            });

            var httpClientFactoryMock = this.Get<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(fakeHttpMessageHandler));

            // Act
            var target = this.Build();
            var act = await target.GetExchangeRates(new List<Currency>
            {
                new Currency("EUR"),
                new Currency("AUD"),
                new Currency("DUMMY"),
                new Currency("TEST"),
                new Currency("USD")
            });

            // Assert
            act.Should().NotBeNullOrEmpty()
                .And.HaveCount(3)
                .And.Contain(x => x.SourceCurrency.Code == "EUR")
                .And.Contain(x => x.SourceCurrency.Code == "AUD")
                .And.NotContain(x => x.SourceCurrency.Code == "DUMMY")
                .And.NotContain(x => x.SourceCurrency.Code == "TEST")
                .And.Contain(x => x.SourceCurrency.Code == "AUD");
        }
    }
}