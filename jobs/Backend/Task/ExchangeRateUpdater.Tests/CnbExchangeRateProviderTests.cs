using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    public class CnbExchangeRateProviderTests
    {
        private const string ApiUrl = "https://cnb.cz/dailyrate";
        
        [Fact]
        public async Task GetExchangeRatesAsync_GivenNullCurrencies_ReturnEmptyExchangeRates()
        {
            // arrange
            var provider = new CnbExchangeRateProvider(new HttpClient(), new Configuration());

            // act
            var rates = await provider.GetExchangeRatesAsync(null);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_GivenEmptyCurrencies_ReturnEmptyExchangeRates()
        {
            // arrange
            var provider = new CnbExchangeRateProvider(new HttpClient(), new Configuration());

            // act
            var rates = await provider.GetExchangeRatesAsync(Enumerable.Empty<Currency>());
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_EmptyResponseString_ReturnEmptyExchangeRates()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent("")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_NoRateInfoResponseString_ReturnEmptyExchangeRates()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent("FirstLine\nSecondLine")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_NoSourceCurrencyResponseString_ReturnEmptyExchangeRates()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent("FirstLine\nSecondLine\nCanada|dollar|1|CAD|17.341")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_InvalidAmountResponseString_ReturnEmptyExchangeRates()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent("FirstLine\nSecondLine\nUSA|dollar|invalid-amount|USD|21.325")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_InvalidRateResponseString_ReturnEmptyExchangeRates()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent("FirstLine\nSecondLine\nUSA|dollar|1|USD|invalid-rate")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Empty(rates);
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_ExistSingleRateResponseString_ReturnSingleExchangeRate()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();

            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent($"FirstLine\nSecondLine\nUSA|dollar|1|USD|21.325")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Single(rates);
            Assert.Collection(
                rates,
                new Action<ExchangeRate>[]
                {
                    exchangeRate =>
                    {
                        Assert.Equal("USD", exchangeRate.SourceCurrency.Code);
                        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
                        Assert.Equal(21.325M, exchangeRate.Value);
                    }
                });
        }
        
        [Fact]
        public async Task GetExchangeRatesAsync_ExistSingleRateWithNonOneAmountResponseString_ReturnSingleExchangeRateWithCalculatedValue()
        {
            // arrange
            var mockConfiguration = GetMockConfiguration();
            
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK, 
                Content = new StringContent($"FirstLine\nSecondLine\nThailand|baht|100|THB|67.037")
            };

            var httpClient = GetHttpClient(httpResponse);
            
            var provider = new CnbExchangeRateProvider(httpClient, mockConfiguration.Object);
            
            var currencies = new[]
            {
                new Currency("USD"),
                new Currency("EUR"),
                new Currency("THB")
            };

            // act
            var rates = await provider.GetExchangeRatesAsync(currencies);
            
            // assert
            Assert.Single(rates);
            Assert.Collection(
                rates,
                new Action<ExchangeRate>[]
                {
                    exchangeRate =>
                    {
                        Assert.Equal("THB", exchangeRate.SourceCurrency.Code);
                        Assert.Equal("CZK", exchangeRate.TargetCurrency.Code);
                        Assert.Equal(0.67037M, exchangeRate.Value);
                    }
                });
        }

        private static Mock<IConfiguration> GetMockConfiguration()
        {
            var mockConfiguration = new Mock<IConfiguration>();
            
            mockConfiguration
                .Setup(configuration => configuration.GetAppSettingValue("CnbApiUrl"))
                .Returns(ApiUrl);

            return mockConfiguration;
        }

        private static HttpClient GetHttpClient(HttpResponseMessage httpResponseMessage)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                    ItExpr.Is<HttpRequestMessage>(request => request.Method == HttpMethod.Get && request.RequestUri.ToString().Equals(ApiUrl)),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(httpResponseMessage);

            return new HttpClient(mockHandler.Object);
        }
    }
}