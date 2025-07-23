using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Text;

namespace ExchangeRateUpdater.Tests.Services
{
    public class ExchangeRateProviderTests
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        private readonly Mock<HttpMessageHandler> _handlerMock;

        private readonly Mock<IOptions<CNBConfigurationOptions>> _optionsMock;
        private readonly ExchangeRateProvider _provider;

        public ExchangeRateProviderTests()
        {
            // Register encoding provider to support windows-1250
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _handlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_handlerMock.Object);
            //_httpClientFactoryMock = new Mock<IHttpClientFactory>();
            //_httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _optionsMock = new Mock<IOptions<CNBConfigurationOptions>>();
            _optionsMock.Setup(o => o.Value).Returns(new CNBConfigurationOptions
            {
                DataURL = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml"
            });
            _provider = new ExchangeRateProvider(_optionsMock.Object, httpClient); // Adjust constructor if using IHttpClientFactory
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
            BuildingEmptyRequest();
            var currencies = new List<Currency>();

            //Act
            var rates = await _provider.GetExchangeRates(currencies);

            //Assert
            Assert.Empty(rates);
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

        private void BuildingEmptyRequest()
        {
            var xmlContent = @"<?xml version='1.0' encoding='windows-1250'?>
            <kurzy_vybrane_meny>
                <kurz id='USD' kurz='23.450' mnozstvi='1' zeme='USA' />
            </kurzy_vybrane_meny>";
            var response = new HttpResponseMessage
            {
                Content = new StringContent(xmlContent, System.Text.Encoding.GetEncoding("windows-1250"))
            };
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);
        }
    }
}
