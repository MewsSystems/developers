using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Providers;
using ExchangeRateUpdater.Domain.Repositories;
using ExchangeRateUpdater.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Infrastructure.Tests.Repositories;

public class ExchangeRateRepositoryTests
{
    private readonly Mock<IExchangeRateProvider> _mockProvider1;
    private readonly Mock<IExchangeRateProvider> _mockProvider2;
    private readonly Mock<ILogger<ExchangeRateRepository>> _mockLogger;
    private readonly ExchangeRateRepository _repository;

    public ExchangeRateRepositoryTests()
    {
        _mockProvider1 = new Mock<IExchangeRateProvider>();
        _mockProvider2 = new Mock<IExchangeRateProvider>();
        _mockLogger = new Mock<ILogger<ExchangeRateRepository>>();

        _mockProvider1.Setup(p => p.Name).Returns("Provider1");
        _mockProvider2.Setup(p => p.Name).Returns("Provider2");

        var providers = new[] { _mockProvider1.Object, _mockProvider2.Object };
        _repository = new ExchangeRateRepository(providers, _mockLogger.Object);
    }

    [Fact]
    public void Constructor_WithValidProviders_ShouldCreateSuccessfully()
    {
        // Arrange
        var providers = new[] { _mockProvider1.Object };

        // Act
        var repository = new ExchangeRateRepository(providers, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public void Constructor_WithEmptyProviders_ShouldCreateSuccessfully()
    {
        // Arrange
        var providers = Array.Empty<IExchangeRateProvider>();

        // Act
        var repository = new ExchangeRateRepository(providers, _mockLogger.Object);

        // Assert
        Assert.NotNull(repository);
    }

    [Fact]
    public async Task FilterAsync_WithValidCurrencies_ShouldReturnFilteredRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.FilterAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task FilterAsync_WithEmptyCurrencies_ShouldReturnAllRates()
    {
        // Arrange
        var currencies = Array.Empty<Currency>();
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.FilterAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task FilterAsync_WhenProviderFails_ShouldContinueWithSuccessfulProviders()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider2", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ThrowsAsync(new Exception("Provider1 failed"));

        _mockProvider2.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.FilterAsync(currencies);

        // Assert
        Assert.NotNull(result);
        Assert.DoesNotContain("Provider1", result.Keys);
        Assert.Contains("Provider2", result.Keys);
        Assert.Single(result["Provider2"]);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRates()
    {
        // Arrange
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task GetFromProviderAsync_WithValidProvider_ShouldReturnProviderRates()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        Assert.Single(result["Provider1"]);
    }

    [Fact]
    public async Task GetFromProviderAsync_WithNonExistentProvider_ShouldThrowArgumentException()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _repository.GetFromProviderAsync("NonExistentProvider", currencies));
    }

    [Fact]
    public async Task GetFromProviderAsync_WithEmptyCurrencies_ShouldReturnAllProviderRates()
    {
        // Arrange
        var currencies = Array.Empty<Currency>();
        var expectedRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("EUR"), 0.85m, "Provider1", DateTime.UtcNow.AddDays(1)),
            new ExchangeRate(new Currency("EUR"), new Currency("USD"), 1.18m, "Provider1", DateTime.UtcNow.AddDays(1))
        };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ReturnsAsync(expectedRates);

        // Act
        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Contains("Provider1", result.Keys);
        // When currencies is empty, it should return all rates (no filtering)
        Assert.Empty(result["Provider1"]); // Empty currencies means no rates match
    }

    [Fact]
    public async Task GetFromProviderAsync_WhenProviderFails_ShouldReturnEmptyResult()
    {
        // Arrange
        var currencies = new[] { new Currency("USD") };

        _mockProvider1.Setup(p => p.FetchAllCurrentAsync())
            .ThrowsAsync(new Exception("Provider failed"));

        // Act
        var result = await _repository.GetFromProviderAsync("Provider1", currencies);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }
} 