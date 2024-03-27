using Castle.Core.Logging;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Infrastructure.Service;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using RichardSzalay.MockHttp;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace ExchangeRateUpdaterTests
{
    public class ExchangeRateServiceTests
    {
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public async Task GetDailyExchangeRates_Returns_DailyExchangeRates()
        {
            //Arrange
            var exchangeRateResponse = GetExchangeRatesResponseTestData();
            var exchangeRateService = GetMockExchangeRateService(exchangeRateResponse, HttpStatusCode.OK);

            //Act
            var result = await exchangeRateService.GetDailyExchangeRates(CancellationToken.None);

            //Assert
            result.Should().BeEquivalentTo(GetExchangeRatesResponseTestData());
        }

        [Test]
        public async Task GetDailyExchangeRates_Returns_NullWhenBadGateway()
        {
            //Arrange
            var exchangeRateResponse = GetExchangeRatesResponseTestData();
            var exchangeRateService = GetMockExchangeRateService(exchangeRateResponse, HttpStatusCode.BadGateway);

            //Act
            var result = await exchangeRateService.GetDailyExchangeRates(CancellationToken.None);

            //Assert
            result.Should().BeNull();
        }

        public ExchangeRateResponse GetExchangeRatesResponseTestData()
        {
            return new ExchangeRateResponse
            {
                Rates = new[]
                {
                    new ExchangeRateData
                    {
                        Amount = 1,
                        Country = "USA",
                        Currency = "dollar",
                        CurrencyCode = "USD",
                        Rate = 23.408M,
                        Order = 62,
                        ValidFor = DateTime.Parse("2024-03-26")
                    },
                    new ExchangeRateData
                    {
                        Amount = 1,
                        Country = "EMU",
                        Currency = "euro",
                        CurrencyCode = "EUR",
                        Rate = 25.32M,
                        Order = 62,
                        ValidFor = DateTime.Parse("2024-03-26")
                    },
                    new ExchangeRateData
                    {
                        Amount = 1,
                        Country = "Denmark",
                        Currency = "krone",
                        CurrencyCode = "DKK",
                        Rate = 3.395M,
                        Order = 62,
                        ValidFor = DateTime.Parse("2024-03-26")
                    }
                }
            };
        }

        public ExchangeRateService GetMockExchangeRateService(ExchangeRateResponse exchangeRateResponse, HttpStatusCode httpStatusCode)
        {
            var handler = new MockHttpMessageHandler();
            handler.When("http://test.cbn.api/*").Respond(httpStatusCode, JsonContent.Create(exchangeRateResponse));

            var httpClient = handler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://test.cbn.api/cnbapi/exrates/daily");

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(httpClient);

            var _logger = new Mock<ILogger<IExchangeRateService>>();
            var _memoryCache = new Mock<IMemoryCache>();
            _memoryCache.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            return new ExchangeRateService(_logger.Object, _memoryCache.Object, mockHttpClientFactory.Object);
        }
    }
}

