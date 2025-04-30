using ExchangeRateProviderAPI_PaolaRojas.Models;
using ExchangeRateProviderAPI_PaolaRojas.Models.Options;
using ExchangeRateProviderAPI_PaolaRojas.Services;
using ExchangeRateProviderAPI_PaolaRojas.UnitTests.Mocks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateProviderAPI_PaolaRojas.UnitTests.Services
{
    public class ExchangeRateServiceTests
    {
        private readonly MockHttpMessageHandler _httpHandler;
        private readonly IMemoryCache _cache;
        private readonly IOptions<CnbOptions> _options;
        private readonly ILogger<ExchangeRateService> _logger;

        private const string MockCnbText = """
            29 Apr 2025 #82
            Country|Currency|Amount|Code|Rate
            USA|dollar|1|USD|21.909
            EMU|euro|1|EUR|24.920
            India|rupee|100|INR|25.732
            BADLINE
            """;

        public ExchangeRateServiceTests()
        {
            _httpHandler = new MockHttpMessageHandler();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _options = Options.Create(new CnbOptions
            {
                DailyExchangeBaseUrl = "https://mock-cnb-url.com"
            });
            _logger = Mock.Of<ILogger<ExchangeRateService>>();
        }

        private ExchangeRateService CreateService()
        {
            _httpHandler.SetMockResponse(MockCnbText);
            var httpClient = new HttpClient(_httpHandler);
            return new ExchangeRateService(_options, _cache, _logger, httpClient);
        }

        [Fact]
        public async Task Should_Return_Filtered_ExchangeRates()
        {
            var service = CreateService();
            var result = await service.GetExchangeRatesAsync([new Currency("USD")]);

            result.ExchangeRates.Should().ContainSingle(e => e.SourceCurrency.Code == "USD");
        }

        [Fact]
        public async Task Should_Return_Empty_When_Currency_Not_Found()
        {
            var service = CreateService();
            var result = await service.GetExchangeRatesAsync([new Currency("XYZ")]);

            result.ExchangeRates.Should().BeEmpty();
        }

        [Fact]
        public async Task Should_Ignore_Malformed_Lines()
        {
            var service = CreateService();
            var result = await service.GetExchangeRatesAsync([new Currency("INR")]);

            result.ExchangeRates.Should().ContainSingle(e => e.SourceCurrency.Code == "INR");
        }

        [Fact]
        public async Task Should_Handle_Duplicate_Currencies_Gracefully()
        {
            var service = CreateService();
            var result = await service.GetExchangeRatesAsync([new Currency("USD"), new Currency("usd"), new Currency("USD")]);

            result.ExchangeRates.Should().ContainSingle(e => e.SourceCurrency.Code == "USD");
        }

        [Fact]
        public async Task Should_Filter_Multiple_Valid_Currencies()
        {
            var service = CreateService();
            var result = await service.GetExchangeRatesAsync([new Currency("USD"), new Currency("EUR"), new Currency("XYZ")]);

            result.ExchangeRates.Should().HaveCount(2);
        }

        [Fact]
        public async Task Should_Return_Cached_Result_On_Second_Call()
        {
            var service = CreateService();
            var first = await service.GetExchangeRatesAsync([new Currency("USD")]);
            var second = await service.GetExchangeRatesAsync([new Currency("USD")]);

            second.Should().BeEquivalentTo(first);
        }

        [Fact]
        public async Task Should_Return_Empty_Response_When_Exception_Occurs()
        {
            var throwingHandler = new MockHttpMessageHandler();
            throwingHandler.SetException(new HttpRequestException("CNB is down"));

            var httpClient = new HttpClient(throwingHandler);
            var service = new ExchangeRateService(_options, _cache, _logger, httpClient);

            var currencies = new[] { new Currency("USD") };

            var result = await service.GetExchangeRatesAsync(currencies);

            result.Should().NotBeNull();
            result.ExchangeRates.Should().BeEmpty();
        }

    }
}