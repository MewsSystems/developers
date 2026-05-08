using ExchangeRateUpdater.Application.DTOs;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Infrastructure.Options;
using ExchangeRateUpdater.Infrastructure.Sources;
using ExchangeRateUpdater.Tests.UnitTests.Data;
using ExchangeRateUpdater.Tests.UnitTests.HttpMessageHandlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Xunit;

namespace ExchangeRateUpdater.Tests.UnitTests
{
    public class CnbApiExchangeRateSourceTest
    {
        private readonly Mock<ILogger<CnbApiExchangeRateSource>> _loggerMock;
        private readonly IConfiguration _configuration;

        public CnbApiExchangeRateSourceTest()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.Test.json", optional: false)
                .Build();
            _loggerMock = new Mock<ILogger<CnbApiExchangeRateSource>>();
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateSourceException_WhenRequestTimesOut()
        {
            // Arrange
            var httpClient = new HttpClient(new TimeoutHttpMessageHandler())
            {
                BaseAddress = new Uri(_configuration["CnbApi:BaseUrl"])
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            // Act
            var ex = await Assert.ThrowsAsync<ExchangeRateSourceException>(() => source.GetDailyExchangeRates());

            // Assert
            Assert.IsType<TaskCanceledException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateSourceException_WhenHttpStatusIsError()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            //Act
            var ex = await Assert.ThrowsAsync<ExchangeRateSourceException>(() => source.GetDailyExchangeRates());

            //Assert
            Assert.IsType<HttpRequestException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateSourceException_WhenJsonIsInvalid()
        {
            //Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("this is not json")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            //Act
            var ex = await Assert.ThrowsAsync<ExchangeRateSourceException>(() => source.GetDailyExchangeRates());

            //Assert
            Assert.IsType<JsonException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateSourceException_WhenJsonIsNull()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("null")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            // Act
            var ex = await Assert.ThrowsAsync<ExchangeRateSourceException>(() => source.GetDailyExchangeRates());

            // Assert
            Assert.IsType<JsonException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ThrowsExchangeRateSourceException_WhenRatesPropertyIsMissing()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonTestDataLoader.Load("CnbApi/dailyRates_noRates.json"), Encoding.UTF8, "application/json")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            // Act
            var ex = await Assert.ThrowsAsync<ExchangeRateSourceException>(() => source.GetDailyExchangeRates());

            // Assert
            Assert.IsType<JsonException>(ex.InnerException);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsEmptyList_WhenRatesArrayIsEmpty()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonTestDataLoader.Load("CnbApi/dailyRates_emptyRates.json"), Encoding.UTF8, "application/json")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(_loggerMock.Object, options, httpClient);

            // Act
            var result = await source.GetDailyExchangeRates();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDailyExchangeRates_ReturnsRates_WhenResponseIsValid()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonTestDataLoader.Load("CnbApi/dailyrates_valid.json"), Encoding.UTF8, "application/json")
            };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(response))
            {
                BaseAddress = new Uri("https://api.cnb.cz/cnbapi/")
            };
            var options = Options.Create(new CnbApiOptions
            {
                DailyRatesPath = "exrates/daily?lang=EN"
            });

            var source = new CnbApiExchangeRateSource(
                _loggerMock.Object,
                options,
                httpClient);

            // Act
            var result = await source.GetDailyExchangeRates();

            // Assert
            var rates = result.ToList();
            Assert.Equal(3, rates.Count);
        }
    }
}