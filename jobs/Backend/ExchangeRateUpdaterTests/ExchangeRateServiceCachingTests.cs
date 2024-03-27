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
    public class ExchangeRateServiceCachingTests
    {
        private Mock<ILogger<IExchangeRateService>> _logger;
        private MemoryCache _memoryCache;
        private ExchangeRateService _exchangeRateService;
        private ExchangeRateResponse exchangeRateResponse;
        private ExchangeRateResponse exchangeRateResponseCached;

        [SetUp]
        public void Setup()
        {
            exchangeRateResponse = GetExchangeRatesResponseTestData();
            exchangeRateResponseCached = GetExchangeRatesResponseCachedTestData();

            var handler = new MockHttpMessageHandler();
            handler.When("http://test.cbn.api/*").Respond(HttpStatusCode.OK, JsonContent.Create(exchangeRateResponse));

            var httpClient = handler.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://test.cbn.api/cnbapi/exrates/daily");

            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            mockHttpClientFactory.Setup(p => p.CreateClient(It.IsAny<string>())).Returns(httpClient);

            _logger = new Mock<ILogger<IExchangeRateService>>();
            _memoryCache = new MemoryCache(new MemoryCacheOptions());

            _memoryCache.Set("DailyExchangeRates", exchangeRateResponseCached, TimeSpan.FromHours(24));
            _exchangeRateService = new ExchangeRateService(_logger.Object, _memoryCache, mockHttpClientFactory.Object);
        }

        [Test]
        public async Task GetDailyExchangeRates_Returns_DailyExchangeRatesFromCache()
        {
            //Act
            var result = await _exchangeRateService.GetDailyExchangeRates(CancellationToken.None);

            //Assert
            result.Should().BeEquivalentTo(exchangeRateResponseCached);
            result.Should().NotBeEquivalentTo(exchangeRateResponse);
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

        public ExchangeRateResponse GetExchangeRatesResponseCachedTestData()
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
                    }
                }
            };
        }
    }
}

