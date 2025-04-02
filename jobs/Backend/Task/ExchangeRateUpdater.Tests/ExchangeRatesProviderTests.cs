using System.Net;
using RichardSzalay.MockHttp;

namespace ExchangeRateUpdater.Tests
{
    public class ExchangeRatesProviderTests
    {
        private static readonly string _uri = "https://www.cnb.cz/*";

        [Fact]
        public void GetExchangeRates_NoCurrencies_ReturnsNoExchangeRates()
        {
            // Arrange
            var currencies = new List<Currency>();
            var mockHttp = new MockHttpMessageHandler();
            var provider = new ExchangeRateProvider(mockHttp.ToHttpClient());

            // Act
            var result = provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetExchangeRates_FoundCurrency_ReturnsCurrencyExchangeRate()
        {
            // Arrange
            var eurCurrency = new Currency("EUR");
            var currencies = new List<Currency>();
            currencies.Add(eurCurrency);
            
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_uri).Respond("text/plain",
                "Currency: EUR|Amount: 1\n" +
                "Date | Rate\n" +
                "26.03.2025|24.930\n" +
                "27.03.2025|24.985\n" +
                "28.03.2025|24.955\n"
            );

            var provider = new ExchangeRateProvider(mockHttp.ToHttpClient());
            var expectedExchangeRate = new ExchangeRate(eurCurrency, new Currency("CZK"), 24.955M);

            // Act
            var result = provider.GetExchangeRates(currencies);

            // Assert
            Assert.Equal(result.First().ToString(), expectedExchangeRate.ToString());
        }

        [Fact]
        public void GetExchangeRates_NotFoundCurrency_ReturnsNoCurrencyExchangeRate()
        {
            // Arrange
            var xyzCurrency = new Currency("XYZ");
            var currencies = new List<Currency>();
            currencies.Add(xyzCurrency);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_uri).Respond("text/plain", "");
            var provider = new ExchangeRateProvider(mockHttp.ToHttpClient());

            // Act
            var result = provider.GetExchangeRates(currencies);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void GetExchangeRates_APIRequestFails_ThrowsHttpRequestException()
        {
            // Arrange
            var eurCurrency = new Currency("EUR");
            var currencies = new List<Currency>();
            currencies.Add(eurCurrency);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_uri).Respond(HttpStatusCode.NotFound);
            var provider = new ExchangeRateProvider(mockHttp.ToHttpClient());

            // Act and Assert
            Assert.Throws<HttpRequestException>(() => provider.GetExchangeRates(currencies));
        }

        [Fact]
        public void GetExchangeRates_APIRequestResponseFormatChanged_ThrowsFormatException()
        {
            // Arrange
            var eurCurrency = new Currency("EUR");
            var currencies = new List<Currency>();
            currencies.Add(eurCurrency);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(_uri).Respond("text/plain",
                "Currency: EUR | Amount: 1\n" +
                "Date | Rate\n" +
                "26.03.2025 | 24.930\n" +
                "27.03.2025 | 24.985\n" +
                "28.03.2025 | 24.955\n"
            );

            var provider = new ExchangeRateProvider(mockHttp.ToHttpClient());

            // Act and Assert
            Assert.Throws<FormatException>(() => provider.GetExchangeRates(currencies));
        }
    }
}