using ExchangeRateUpdater;
using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Repositories;
using Moq;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateRepository> _mockRepository;
    private readonly ExchangeRateProvider _provider;

    public ExchangeRateProviderTests()
    {
        _mockRepository = new Mock<IExchangeRateRepository>();
        _provider = new ExchangeRateProvider(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_WithValidRepository_ShouldCreateSuccessfully()
    {
        var provider = new ExchangeRateProvider(_mockRepository.Object);
        Assert.NotNull(provider);
    }

    [Fact]
    public async Task GetExchangeRates_WithValidCurrencies_ShouldReturnExchangeRates()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var provider1Rates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m, "CNB")
        };
        var provider2Rates = new[]
        {
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.2m, "CNB")
        };

        var repositoryResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "CzechNationalBank", provider1Rates.Concat(provider2Rates).ToArray() }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(repositoryResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Contains(result, r => r.SourceCurrency.Code == "USD" && r.TargetCurrency.Code == "CZK");
        Assert.Contains(result, r => r.SourceCurrency.Code == "EUR" && r.TargetCurrency.Code == "CZK");
    }

    [Fact]
    public async Task GetExchangeRates_WithEmptyRepositoryResponse_ShouldReturnEmptyArray()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WithNullCurrencies_ShouldReturnEmptyArray()
    {
        IEnumerable<Currency>? currencies = null;
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WithEmptyCurrencies_ShouldReturnEmptyArray()
    {
        var currencies = Array.Empty<Currency>();
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WithMultipleProviders_ShouldReturnFirstProviderRates()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var provider1Rates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m, "CNB")
        };
        var provider2Rates = new[]
        {
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.2m, "OtherProvider")
        };

        var repositoryResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "CzechNationalBank", provider1Rates },
            { "OtherProvider", provider2Rates }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(repositoryResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("USD", result.First().SourceCurrency.Code);
        Assert.Equal("CZK", result.First().TargetCurrency.Code);
        Assert.Equal(23.5m, result.First().ExchangeValue);
    }

    [Fact]
    public async Task GetExchangeRates_WithRepositoryThrowingException_ShouldPropagateException()
    {
        var currencies = new[] { new Currency("USD") };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ThrowsAsync(new Exception("Repository error"));

        await Assert.ThrowsAsync<Exception>(() => _provider.GetExchangeRates(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithValidCurrencies_ShouldReturnAllProviders()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var provider1Rates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m, "CNB")
        };
        var provider2Rates = new[]
        {
            new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 25.2m, "OtherProvider")
        };

        var expectedResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "CzechNationalBank", provider1Rates },
            { "OtherProvider", provider2Rates }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(expectedResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains("CzechNationalBank", result.Keys);
        Assert.Contains("OtherProvider", result.Keys);
        Assert.Single(result["CzechNationalBank"]);
        Assert.Single(result["OtherProvider"]);
        Assert.Equal("USD", result["CzechNationalBank"][0].SourceCurrency.Code);
        Assert.Equal("EUR", result["OtherProvider"][0].SourceCurrency.Code);
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithEmptyRepositoryResponse_ShouldReturnEmptyDictionary()
    {
        var currencies = new[] { new Currency("USD"), new Currency("EUR") };
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithNullCurrencies_ShouldReturnEmptyDictionary()
    {
        IEnumerable<Currency>? currencies = null;
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithEmptyCurrencies_ShouldReturnEmptyDictionary()
    {
        var currencies = Array.Empty<Currency>();
        var emptyResponse = new Dictionary<string, ExchangeRate[]>();

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(emptyResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithRepositoryThrowingException_ShouldPropagateException()
    {
        var currencies = new[] { new Currency("USD") };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ThrowsAsync(new Exception("Repository error"));

        await Assert.ThrowsAsync<Exception>(() => _provider.GetExchangeRatesFromMultipleProviders(currencies));
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithSingleProvider_ShouldReturnSingleProvider()
    {
        var currencies = new[] { new Currency("USD") };
        var providerRates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m, "CNB")
        };

        var expectedResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "CzechNationalBank", providerRates }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(expectedResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Contains("CzechNationalBank", result.Keys);
        Assert.Single(result["CzechNationalBank"]);
        Assert.Equal("USD", result["CzechNationalBank"][0].SourceCurrency.Code);
        Assert.Equal("CZK", result["CzechNationalBank"][0].TargetCurrency.Code);
        Assert.Equal(23.5m, result["CzechNationalBank"][0].ExchangeValue);
    }

    [Fact]
    public async Task GetExchangeRatesFromMultipleProviders_WithProviderHavingEmptyRates_ShouldIncludeEmptyProvider()
    {
        var currencies = new[] { new Currency("USD") };
        var provider1Rates = new[]
        {
            new ExchangeRate(new Currency("USD"), new Currency("CZK"), 23.5m, "CNB")
        };
        var provider2Rates = Array.Empty<ExchangeRate>();

        var expectedResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "CzechNationalBank", provider1Rates },
            { "EmptyProvider", provider2Rates }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(expectedResponse);

        var result = await _provider.GetExchangeRatesFromMultipleProviders(currencies);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Contains("CzechNationalBank", result.Keys);
        Assert.Contains("EmptyProvider", result.Keys);
        Assert.Single(result["CzechNationalBank"]);
        Assert.Empty(result["EmptyProvider"]);
    }

    [Fact]
    public async Task GetExchangeRates_WithProviderHavingEmptyRates_ShouldReturnEmptyArray()
    {
        var currencies = new[] { new Currency("USD") };
        var emptyRates = Array.Empty<ExchangeRate>();

        var repositoryResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "EmptyProvider", emptyRates }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(repositoryResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetExchangeRates_WithProviderHavingNullRates_ShouldReturnNull()
    {
        var currencies = new[] { new Currency("USD") };
        ExchangeRate[]? nullRates = null;

        var repositoryResponse = new Dictionary<string, ExchangeRate[]>
        {
            { "NullProvider", nullRates! }
        };

        _mockRepository.Setup(r => r.FilterAsync(currencies, It.IsAny<DateTime?>()))
            .ReturnsAsync(repositoryResponse);

        var result = await _provider.GetExchangeRates(currencies);

        Assert.Null(result);
    }
} 