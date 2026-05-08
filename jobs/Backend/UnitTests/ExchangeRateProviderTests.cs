using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRateUpdater.UnitTests;

public class ExchangeRateProviderTests
{
    private readonly Mock<ICnbApiClient> _apiClientMock;
    private readonly Mock<ICnbDataParser> _dataParserMock;
    private readonly Mock<ILogger<ExchangeRateProvider>> _loggerMock;
    private readonly Mock<IExchangeRateCache> _cacheMock;
    private readonly IOptions<CnbExchangeRateConfiguration> _configuration;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _apiClientMock = new Mock<ICnbApiClient>();
        _dataParserMock = new Mock<ICnbDataParser>();
        _loggerMock = new Mock<ILogger<ExchangeRateProvider>>();
        _cacheMock = new Mock<IExchangeRateCache>();
        _configuration = Options.Create(new CnbExchangeRateConfiguration
        {
            EnableCache = false // Disable cache for tests by default
        });

        _provider = new ExchangeRateProvider(
            _apiClientMock.Object,
            _dataParserMock.Object,
            _loggerMock.Object,
            _configuration,
            _cacheMock.Object);
    }

    [Fact]
    public void GetExchangeRates_WithValidCurrencies_ReturnsExchangeRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var rawData = "sample data";

        var cnbRates = new List<CnbExchangeRateDto>
        {
            new() { Code = "USD", Amount = 1, Rate = 22.950m, Country = "USA", CurrencyName = "dollar" },
            new() { Code = "EUR", Amount = 1, Rate = 24.375m, Country = "EMU", CurrencyName = "euro" },
            new() { Code = "GBP", Amount = 1, Rate = 28.123m, Country = "UK", CurrencyName = "pound" }
        };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rawData);

        _dataParserMock.Setup(x => x.Parse(rawData))
            .Returns(cnbRates);

        // Act
        var result = _provider.GetExchangeRates(currencies).ToList();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain(r => r.SourceCurrency.Code == "USD");
        result.Should().Contain(r => r.SourceCurrency.Code == "EUR");
        result.Should().NotContain(r => r.SourceCurrency.Code == "GBP");

        result.ForEach(r => r.TargetCurrency.Code.Should().Be("CZK"));
    }

    [Fact]
    public void GetExchangeRates_WithNoCurrencies_ReturnsEmpty()
    {
        // Arrange
        var currencies = Array.Empty<Currency>();

        // Act
        var result = _provider.GetExchangeRates(currencies);

        // Assert
        result.Should().BeEmpty();
        _apiClientMock.Verify(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public void GetExchangeRates_WithNullCurrencies_ThrowsArgumentNullException()
    {
        // Act & Assert
        _provider.Invoking(p => p.GetExchangeRates(null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetExchangeRates_WhenApiClientThrows_ThrowsExchangeRateProviderException()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ExchangeRateProviderException("API error"));

        // Act & Assert
        _provider.Invoking(p => p.GetExchangeRates(currencies))
            .Should().Throw<ExchangeRateProviderException>()
            .WithMessage("API error");
    }

    [Fact]
    public async Task GetExchangeRatesAsync_NormalizesRatesPerUnit()
    {
        // Arrange - CNB provides some rates for multiple units (e.g., 100 JPY)
        var currencies = new[] { new Currency("JPY"), new Currency("USD") };
        var rawData = "sample data";

        var cnbRates = new List<CnbExchangeRateDto>
        {
            new() { Code = "JPY", Amount = 100, Rate = 1523.4m, Country = "Japan", CurrencyName = "yen" },
            new() { Code = "USD", Amount = 1, Rate = 22.950m, Country = "USA", CurrencyName = "dollar" }
        };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rawData);

        _dataParserMock.Setup(x => x.Parse(rawData))
            .Returns(cnbRates);

        // Act
        var result = await _provider.GetExchangeRatesAsync(currencies);
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(2);

        var jpyRate = resultList.First(r => r.SourceCurrency.Code == "JPY");
        jpyRate.Value.Should().Be(15.234m); // 1523.4 / 100

        var usdRate = resultList.First(r => r.SourceCurrency.Code == "USD");
        usdRate.Value.Should().Be(22.950m); // 22.950 / 1
    }

    [Fact]
    public void GetExchangeRates_CaseInsensitiveCurrencyMatching()
    {
        // Arrange
        var currencies = new[] { new Currency("usd"), new Currency("EUR") };
        var rawData = "sample data";

        var cnbRates = new List<CnbExchangeRateDto>
        {
            new() { Code = "USD", Amount = 1, Rate = 22.950m, Country = "USA", CurrencyName = "dollar" },
            new() { Code = "eur", Amount = 1, Rate = 24.375m, Country = "EMU", CurrencyName = "euro" }
        };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rawData);

        _dataParserMock.Setup(x => x.Parse(rawData))
            .Returns(cnbRates);

        // Act
        var result = _provider.GetExchangeRates(currencies).ToList();

        // Assert
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetSupportedCurrenciesAsync_ReturnsAllCurrencyCodes()
    {
        // Arrange
        var rawData = "sample data";
        var cnbRates = new List<CnbExchangeRateDto>
        {
            new() { Code = "USD", Amount = 1, Rate = 22.950m, Country = "USA", CurrencyName = "dollar" },
            new() { Code = "EUR", Amount = 1, Rate = 24.375m, Country = "EMU", CurrencyName = "euro" },
            new() { Code = "GBP", Amount = 1, Rate = 28.123m, Country = "UK", CurrencyName = "pound" }
        };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rawData);

        _dataParserMock.Setup(x => x.Parse(rawData))
            .Returns(cnbRates);

        // Act
        var result = await _provider.GetSupportedCurrenciesAsync();
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(3);
        resultList.Should().Contain("EUR");
        resultList.Should().Contain("GBP");
        resultList.Should().Contain("USD");
        resultList.Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task GetSupportedCurrenciesAsync_WhenApiClientThrows_ThrowsExchangeRateProviderException()
    {
        // Arrange
        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new HttpRequestException("Network error"));

        // Act & Assert
        await _provider.Invoking(p => p.GetSupportedCurrenciesAsync())
            .Should().ThrowAsync<ExchangeRateProviderException>()
            .WithMessage("Failed to retrieve supported currencies");
    }

    [Fact]
    public async Task GetSupportedCurrenciesAsync_WithCachingEnabled_UsesCachedData()
    {
        // Arrange
        var configuration = Options.Create(new CnbExchangeRateConfiguration
        {
            EnableCache = true
        });

        var supportedCurrenciesCacheMock = new Mock<ISupportedCurrenciesCache>();
        var cachedCurrencies = new List<string> { "EUR", "USD" };

        supportedCurrenciesCacheMock.Setup(x => x.GetCachedCurrencies())
            .Returns(cachedCurrencies);

        var provider = new ExchangeRateProvider(
            _apiClientMock.Object,
            _dataParserMock.Object,
            _loggerMock.Object,
            configuration,
            _cacheMock.Object,
            supportedCurrenciesCacheMock.Object);

        // Act
        var result = await provider.GetSupportedCurrenciesAsync();
        var resultList = result.ToList();

        // Assert
        resultList.Should().HaveCount(2);
        resultList.Should().Contain("EUR");
        resultList.Should().Contain("USD");
        _apiClientMock.Verify(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetSupportedCurrenciesAsync_WithCachingEnabled_CachesResults()
    {
        // Arrange
        var configuration = Options.Create(new CnbExchangeRateConfiguration
        {
            EnableCache = true
        });

        var supportedCurrenciesCacheMock = new Mock<ISupportedCurrenciesCache>();

        supportedCurrenciesCacheMock.Setup(x => x.GetCachedCurrencies())
            .Returns((IEnumerable<string>?)null);

        var provider = new ExchangeRateProvider(
            _apiClientMock.Object,
            _dataParserMock.Object,
            _loggerMock.Object,
            configuration,
            _cacheMock.Object,
            supportedCurrenciesCacheMock.Object);

        var rawData = "sample data";
        var cnbRates = new List<CnbExchangeRateDto>
        {
            new() { Code = "USD", Amount = 1, Rate = 22.950m, Country = "USA", CurrencyName = "dollar" }
        };

        _apiClientMock.Setup(x => x.FetchExchangeRatesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(rawData);

        _dataParserMock.Setup(x => x.Parse(rawData))
            .Returns(cnbRates);

        // Act
        await provider.GetSupportedCurrenciesAsync();

        // Assert
        supportedCurrenciesCacheMock.Verify(x => x.SetCachedCurrencies(It.IsAny<IEnumerable<string>>()), Times.Once);
    }
}
