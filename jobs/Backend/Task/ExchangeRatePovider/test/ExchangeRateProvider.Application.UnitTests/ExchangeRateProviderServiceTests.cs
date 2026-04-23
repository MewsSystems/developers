using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Application.Services;
using ExchangeRateProvider.Domain.Entities;
using Moq;

namespace ExchangeRateProvider.Application.UnitTests;

public class ExchangeRateProviderServiceTests
{
    private readonly Mock<IExchangeRateProvider> _mockExchangeRateProvider;
    private readonly ExchangeRateProviderService _service;

    public ExchangeRateProviderServiceTests()
    {
        _mockExchangeRateProvider = new Mock<IExchangeRateProvider>();
        _service = new ExchangeRateProviderService(_mockExchangeRateProvider.Object);
    }

    [Fact]
    public async Task GetLatestAsync_WithValidCurrencies_ReturnsFilteredExchangeRates()
    {
        // Arrange
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var gbp = new Currency("GBP");
        var czk = new Currency("CZK");
        
        var allExchangeRates = new List<ExchangeRate>
        {
            new(usd, czk, 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(eur, czk, 24.8m, DateOnly.FromDateTime(DateTime.Today)),
            new(gbp, czk, 31.2m, DateOnly.FromDateTime(DateTime.Today)),
        };

        var requestedCurrencies = new[] { usd, eur };

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allExchangeRates);

        // Act
        var result = await _service.GetLatestAsync(requestedCurrencies);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, rate => Assert.Contains(rate.SourceCurrency, requestedCurrencies));
        Assert.Contains(result, rate => rate.SourceCurrency == usd);
        Assert.Contains(result, rate => rate.SourceCurrency == eur);
    }

    [Fact]
    public async Task GetLatestAsync_WithEmptyCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var allExchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(new Currency("EUR"), new Currency("CZK"), 24.8m, DateOnly.FromDateTime(DateTime.Today))
        };

        var emptyCurrencies = Array.Empty<Currency>();

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allExchangeRates);

        // Act
        var result = await _service.GetLatestAsync(emptyCurrencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLatestAsync_WithNonMatchingCurrencies_ReturnsEmptyList()
    {
        // Arrange
        var allExchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(new Currency("EUR"), new Currency("CZK"), 24.8m, DateOnly.FromDateTime(DateTime.Today))
        };

        var requestedCurrencies = new[] { new Currency("GBP"), new Currency("JPY") };

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allExchangeRates);

        // Act
        var result = await _service.GetLatestAsync(requestedCurrencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLatestAsync_WithDuplicateCurrencies_ReturnsUniqueResults()
    {
        // Arrange
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        
        var allExchangeRates = new List<ExchangeRate>
        {
            new(usd, new Currency("CZK"), 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(eur, new Currency("CZK"), 24.8m, DateOnly.FromDateTime(DateTime.Today))
        };

        var requestedCurrencies = new[] { usd, usd, eur }; // USD duplicated

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(allExchangeRates);

        // Act
        var result = await _service.GetLatestAsync(requestedCurrencies);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Single(result, rate => rate.SourceCurrency == usd);
        Assert.Single(result, rate => rate.SourceCurrency == eur);
    }

    [Fact]
    public async Task GetLatestAsync_WithEmptyProviderResponse_ReturnsEmptyList()
    {
        // Arrange
        var requestedCurrencies = new[] { new Currency("USD"), new Currency("EUR") };

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var result = await _service.GetLatestAsync(requestedCurrencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetLatestAsync_WithCancellationToken_PassesToProvider()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;
        var requestedCurrencies = new[] { new Currency("USD") };

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(cancellationToken))
            .ReturnsAsync([]);

        // Act
        await _service.GetLatestAsync(requestedCurrencies, cancellationToken);

        // Assert
        _mockExchangeRateProvider.Verify(
            x => x.GetLatestAsync(cancellationToken), 
            Times.Once);
    }

    [Fact]
    public async Task GetLatestAsync_WhenProviderThrows_PropagatesException()
    {
        // Arrange
        var requestedCurrencies = new[] { new Currency("USD") };
        var expectedException = new InvalidOperationException("Provider error");

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.GetLatestAsync(requestedCurrencies));
        
        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task GetLatestAsync_AlwaysCallsProviderOnce()
    {
        // Arrange
        var requestedCurrencies = new[] { new Currency("USD"), new Currency("EUR") };

        _mockExchangeRateProvider
            .Setup(x => x.GetLatestAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        await _service.GetLatestAsync(requestedCurrencies);

        // Assert
        _mockExchangeRateProvider.Verify(
            x => x.GetLatestAsync(It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}
