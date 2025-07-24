using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;

namespace ExchangeRateUpdater.Tests.Services
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;
        private readonly Mock<IOptions<CNBConfigurationOptions>> _optionsMock;

        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object);

            _optionsMock = new Mock<IOptions<CNBConfigurationOptions>>();
            _optionsMock.Setup(o => o.Value).Returns(new CNBConfigurationOptions
            {
                DataURL = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml"
            });

            _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();

            _provider = new ExchangeRateProvider(_optionsMock.Object, httpClient, _loggerMock.Object);
        }

        [Fact]
        public async Task GetExchangeRates_ReturnsExpectedRates()
        {
            //Arrange
            BuildingRequestSucceed();
            var currencies = new List<Currency> { new("USD"), new("EUR") };

            //Act
            var rates = await _provider.GetExchangeRates(currencies);

            // Assert
            var rateList = rates.ToList();
            Assert.Equal(2, rateList.Count);
            Assert.Contains(rateList, r => r.TargetCurrency.Code == "USD" && r.Value == 20.988m);
            Assert.Contains(rateList, r => r.TargetCurrency.Code == "EUR" && r.Value == 24.615m);
        }

        [Fact]
        public async Task GetExchangeRates_EmptyCurrency_ReturnsEmpty()
        {
            //Arrange
            var currencies = new List<Currency>();

            //Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _provider.GetExchangeRates(currencies));
            Assert.Equal("currencies", exception.ParamName);

            _loggerMock.Verify(logger => logger.Log(LogLevel.Error, 
                It.IsAny<EventId>(), 
                It.Is<It.IsAnyType>((v, t) => true), 
                It.IsAny<Exception>(), 
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once());
        }

        [Fact]
        public async Task GetExchangeRates_NoMatchingCurrencies_ReturnsEmpty()
        {
            //Arrange
            BuildingRequestSucceed();
            var currencies = new List<Currency> { new("CAD")};

            //Act
            var rates = await _provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(rates);
        }

        [Fact]
        public async Task GetExchangeRates_HttpRequestFailure_LogsErrorAndReturnsEmpty()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.NotFound
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var currencies = new List<Currency> { new ("USD") };

            // Act
            var rates = await _provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(rates);
            _loggerMock.Verify(x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("HTTP request failed")),
                It.IsAny<HttpRequestException>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once());
        }

        private void BuildingRequestSucceed()
        {
            var xmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
                <kurzy banka=""CNB"" datum=""23.07.2025"" poradi=""141"">
                    <tabulka typ=""XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU"">
                        <radek kod=""EUR"" mena=""euro"" mnozstvi=""1"" kurz=""24,615"" zeme=""EMU""/>
                        <radek kod=""USD"" mena=""dolar"" mnozstvi=""1"" kurz=""20,988"" zeme=""USA""/>
                        <radek kod=""GBP"" mena=""libra"" mnozstvi=""1"" kurz=""28,405"" zeme=""Velká Británie""/>
                    </tabulka>
                </kurzy>";
            var response = new HttpResponseMessage
            {
                Content = new StringContent(xmlContent, Encoding.UTF8, "application/xml")
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }
    }
}
