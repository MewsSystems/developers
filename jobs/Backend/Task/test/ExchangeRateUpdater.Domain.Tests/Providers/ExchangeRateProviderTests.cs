using ExchangeRateUpdater.Clients.Cnb;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Domain.Caches;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Tests.Shared;
using ExchangeRateUpdater.Tests.Shared.Builders;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.Domain.Tests.Providers;

public class ExchangeRateProviderTests : TestBase
{
    private readonly Mock<ICnbClient> _client;
    private readonly Mock<IExchangeRateCache> _cache;
    private ExchangeRateProvider _exchangeRateProvider;

    public ExchangeRateProviderTests()
    {
        _client = new Mock<ICnbClient>();
        _cache = new Mock<IExchangeRateCache>();
        _exchangeRateProvider =
            new ExchangeRateProvider(_client.Object, Mapper, _cache.Object, ApplicationOptions.Object);
    }

    [Fact]
    public async Task
        Given_currencies_when_retrieving_exchange_rates_with_in_memory_cache_and_cache_is_proper_then_return_ok_with_appropriate_content()
    {
        //Arrange
        _cache.Setup(cache => cache.Get()).Returns(new List<ExchangeRate>
        {
            new ExchangeRateBuilder().WithValue(1).WithSourceCurrency("USD")
                .WithTargetCurrency("CZK").Build()
        });

        //Act
        var result = await _exchangeRateProvider.GetExchangeRates(new List<Currency>
        {
            new("USD")
        });

        //Assert
        result.Count().Should().Be(1);
        result.First().Value.Should().Be(1);
        result.First().SourceCurrency.Code.Should().Be("USD");
        result.First().TargetCurrency.Code.Should().Be("CZK");
    }

    [Fact]
    public async Task
        Given_currencies_when_retrieving_exchange_rates_with_in_memory_cache_but_cache_is_empty_then_return_ok_with_appropriate_content()
    {
        //Arrange
        _cache.Setup(cache => cache.Get()).Returns((IEnumerable<ExchangeRate>?)null);
        _client.Setup(client => client.GetExchangeRatesAsync()).ReturnsAsync(new ExchangeRateResponseBuilder()
            .WithCurrentDate(DateTime.Now).WithExchangeRates(
                new List<ExchangeRateDto?>
                {
                    new ExchangeRateDtoBuilder().WithAmount(1).WithCode("USD").WithCurrency("dollar")
                        .WithCountry("Australia").WithRate(1).Build()
                })
            .Build());

        //Act
        var result = await _exchangeRateProvider.GetExchangeRates(new List<Currency>
        {
            new("USD")
        });

        //Assert
        result.Count().Should().Be(1);
        result.First().Value.Should().Be(1);
        result.First().SourceCurrency.Code.Should().Be("USD");
        result.First().TargetCurrency.Code.Should().Be("CZK");
    }

    [Fact]
    public async Task
        Given_currencies_when_retrieving_exchange_rates_without_in_memory_cache_then_return_ok_with_appropriate_content()
    {
        //Arrange
        _client.Setup(client => client.GetExchangeRatesAsync()).ReturnsAsync(new ExchangeRateResponseBuilder()
            .WithCurrentDate(DateTime.Now).WithExchangeRates(
                new List<ExchangeRateDto?>
                {
                    new ExchangeRateDtoBuilder().WithAmount(1).WithCode("USD").WithCurrency("dollar")
                        .WithCountry("Australia").WithRate(1).Build()
                })
            .Build());

        var applicationOptions = new ApplicationOptions
        {
            ExchangeRateCurrency = "CZK",
            EnableInMemoryCache = false
        };

        ApplicationOptions = new Mock<IOptions<ApplicationOptions>>();
        ApplicationOptions.Setup(ap => ap.Value).Returns(applicationOptions);
        _exchangeRateProvider =
            new ExchangeRateProvider(_client.Object, Mapper, _cache.Object, ApplicationOptions.Object);

        //Act
        var result = await _exchangeRateProvider.GetExchangeRates(new List<Currency>
        {
            new("USD")
        });

        //Assert
        result.Count().Should().Be(1);
        result.First().Value.Should().Be(1);
        result.First().SourceCurrency.Code.Should().Be("USD");
        result.First().TargetCurrency.Code.Should().Be("CZK");
    }
}