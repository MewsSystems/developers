using ExchangeRateUpdater.Service;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Contrib.HttpClient;
using System.Reflection;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRateProviderTest
    {
        private readonly string ValidCnbExchangeRatesUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";

        [Theory]
        [InlineData("USD", "22,120")]
        [InlineData("EUR", "23,925")]
        [InlineData("CHF", "24,114")]
        public async Task Test_Get_Available_ExchangeRate(string currency, string rate)
        {
            //Arrange
            ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency> { new Currency(currency) });

            //Assert
            result.Should().Contain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.ToString() == rate);
        }

        [Theory]
        [InlineData("PHP", "0,40479")]
        [InlineData("IDR", "0,001465")]
        [InlineData("THB", "0,66815")]
        public async Task Test_Get_Available_ExchangeRate_Fractioned(string currency, string rate)
        {
            //Arrange
            ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency> { new Currency(currency) });

            //Assert
            result.Should().Contain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase)
                && x.Value.ToString() == rate);
        }

        [Theory]
        [InlineData("AAA")]
        [InlineData("BBB")]
        [InlineData("CCC")]
        public async Task Test_Get_Not_Available_ExchangeRate(string currency)
        {
            //Arrange
            ExchangeRateProvider exchangeRateProvider = GetExchangeRateProviderMock();

            //Act
            var result = await exchangeRateProvider.GetExchangeRates(new List<Currency> { new Currency(currency) });

            //Assert
            result.Should().NotContain(x => x.SourceCurrency.Code.Equals(currency, StringComparison.CurrentCultureIgnoreCase));
        }

        private ExchangeRateProvider GetExchangeRateProviderMock()
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.SetupAnyRequest()
                .ReturnsResponse(System.Net.HttpStatusCode.OK, File.ReadAllText("mock_exchange_rate_response.xml"), "text/xml");

            var factory = handler.CreateClientFactory();

            var settings = new Dictionary<string, string> {
                {"cnbExchangeRatesUrl", ValidCnbExchangeRatesUrl}
            };

            IConfiguration configuration = new ConfigurationBuilder()
              .AddInMemoryCollection(settings)
              .Build();

            var exchangeRateProvider = new ExchangeRateProvider(factory, configuration);
            return exchangeRateProvider;
        }
    }
}