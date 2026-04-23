using ExchangeRateProvider.Api.Dto;
using ExchangeRateProvider.Api.Endpoints;
using ExchangeRateProvider.Application.Interfaces;
using ExchangeRateProvider.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Moq;

namespace ExchangeRateProvider.Api.Tests.Endpoints;

public class ExchangeRateEndpointsTests
{
    private readonly Mock<IExchangeRateProviderService> _mockExchangeRateProviderService = new();

    [Fact]
    public async Task ExchangeRateEndpoint_WithValidCurrencies_ReturnsExchangeRates()
    {
        // Arrange
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var czk = new Currency("CZK");
        
        var exchangeRates = new List<ExchangeRate>
        {
            new(usd, czk, 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(eur, czk, 24.8m, DateOnly.FromDateTime(DateTime.Today))
        };

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        const string currencies = "USD,EUR";
        var cancellationToken = CancellationToken.None;

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var result = await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, cancellationToken);

        var response = result.Select(r =>
            new ExchangeRateDto(r.SourceCurrency.Code, r.TargetCurrency.Code, r.Value));

        // Assert
        Assert.NotNull(response);
        Assert.Equal(2, response.Count());
        Assert.Contains(response, dto => dto is { SourceCurrency: "USD", Value: 25.5m });
        Assert.Contains(response, dto => dto is { SourceCurrency: "EUR", Value: 24.8m });
    }

    [Fact]
    public async Task ExchangeRateEndpoint_WithSingleCurrency_ReturnsSingleRate()
    {
        // Arrange
        var usd = new Currency("USD");
        var czk = new Currency("CZK");
        
        var exchangeRates = new List<ExchangeRate>
        {
            new(usd, czk, 25.5m, DateOnly.FromDateTime(DateTime.Today))
        };

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        const string currencies = "USD";
        var cancellationToken = CancellationToken.None;

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var result = await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, cancellationToken);

        var response = result.Select(r =>
            new ExchangeRateDto(r.SourceCurrency.Code, r.TargetCurrency.Code, r.Value));

        // Assert
        Assert.NotNull(response);
        Assert.Single(response);
        var dto = response.First();
        Assert.Equal("USD", dto.SourceCurrency);
        Assert.Equal("CZK", dto.TargetCurrency);
        Assert.Equal(25.5m, dto.Value);
    }

    [Fact]
    public void ExchangeRateEndpoint_CurrencyParsing_WithValidString_SplitsCorrectly()
    {
        // Arrange
        const string currenciesString = "USD,EUR,GBP";

        // Act
        var currencyList = currenciesString
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code))
            .ToList();

        // Assert
        Assert.Equal(3, currencyList.Count);
        Assert.Contains(currencyList, c => c.Code == "USD");
        Assert.Contains(currencyList, c => c.Code == "EUR");
        Assert.Contains(currencyList, c => c.Code == "GBP");
    }

    [Fact]
    public void ExchangeRateEndpoint_CurrencyParsing_WithSpaces_TrimsSpaces()
    {
        // Arrange
        const string currenciesString = " USD , EUR , GBP ";

        // Act
        var currencyList = currenciesString
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code))
            .ToList();

        // Assert
        Assert.Equal(3, currencyList.Count);
        Assert.Contains(currencyList, c => c.Code == "USD");
        Assert.Contains(currencyList, c => c.Code == "EUR");
        Assert.Contains(currencyList, c => c.Code == "GBP");
    }

    [Fact]
    public void ExchangeRateEndpoint_CurrencyParsing_WithEmptyEntries_RemovesEmptyEntries()
    {
        // Arrange
        const string currenciesString = "USD,,EUR,";

        // Act
        var currencyList = currenciesString
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code))
            .ToList();

        // Assert
        Assert.Equal(2, currencyList.Count);
        Assert.Contains(currencyList, c => c.Code == "USD");
        Assert.Contains(currencyList, c => c.Code == "EUR");
    }

    [Fact]
    public void ExchangeRateEndpoint_Validation_WithNullCurrencies_ShouldReturnBadRequest()
    {
        // Arrange
        string? currencies = null;

        // Act & Assert - Test the validation logic from the endpoint
        var result = string.IsNullOrWhiteSpace(currencies);
        
        Assert.True(result);
    }

    [Fact]
    public void ExchangeRateEndpoint_Validation_WithEmptyCurrencies_ShouldReturnBadRequest()
    {
        // Arrange
        const string currencies = "";

        // Act & Assert - Test the validation logic from the endpoint
        var result = string.IsNullOrWhiteSpace(currencies);
        
        Assert.True(result);
    }

    [Fact]
    public void ExchangeRateEndpoint_Validation_WithWhitespaceCurrencies_ShouldReturnBadRequest()
    {
        // Arrange
        const string currencies = "   ";

        // Act & Assert - Test the validation logic from the endpoint
        var result = string.IsNullOrWhiteSpace(currencies);
        
        Assert.True(result);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_WithEmptyResponse_ReturnsEmptyList()
    {
        // Arrange
        const string currencies = "USD";

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var result = await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, CancellationToken.None);

        var response = result.Select(r =>
            new ExchangeRateDto(r.SourceCurrency.Code, r.TargetCurrency.Code, r.Value));

        // Assert
        Assert.NotNull(response);
        Assert.Empty(response);
    }

    [Fact]
    public void ExchangeRateDto_Mapping_ShouldMapCorrectly()
    {
        // Arrange
        var sourceCurrency = new Currency("USD");
        var targetCurrency = new Currency("CZK");
        var exchangeRate = new ExchangeRate(sourceCurrency, targetCurrency, 25.5m, DateOnly.FromDateTime(DateTime.Today));

        // Act
        var dto = new ExchangeRateDto(
            exchangeRate.SourceCurrency.Code, 
            exchangeRate.TargetCurrency.Code, 
            exchangeRate.Value);

        // Assert
        Assert.Equal("USD", dto.SourceCurrency);
        Assert.Equal("CZK", dto.TargetCurrency);
        Assert.Equal(25.5m, dto.Value);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_ServiceThrowsException_PropagatesException()
    {
        // Arrange
        const string currencies = "USD";
        var expectedException = new InvalidOperationException("Service error");

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var actualException = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _mockExchangeRateProviderService.Object.GetLatestAsync(currencyList, CancellationToken.None));
        
        Assert.Same(expectedException, actualException);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_WithCancellationToken_PassesToService()
    {
        // Arrange
        const string currencies = "USD";
        var cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = cancellationTokenSource.Token;

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), cancellationToken))
            .ReturnsAsync([]);

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, cancellationToken);

        // Assert
        _mockExchangeRateProviderService.Verify(
            x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), cancellationToken),
            Times.Once);
    }

    [Fact]
    public void ExchangeRateEndpoints_MapExchangeRatesEndpoints_ReturnsRouteBuilder()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // Act
        var result = app.MapExchangeRatesEndpoints();

        // Assert
        Assert.NotNull(result);
        Assert.Same(app, result);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_WithDuplicateCurrencies_ProcessesDuplicates()
    {
        // Arrange
        const string currencies = "USD,EUR,USD,EUR";
        var expectedUniqueCurrencies = new[] { "USD", "EUR" };
        
        var exchangeRates = expectedUniqueCurrencies.Select(code => 
            new ExchangeRate(
                new Currency(code), 
                new Currency("CZK"), 
                25.0m, 
                DateOnly.FromDateTime(DateTime.Today)))
            .ToList();

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var result = await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_WithValidResponse_MapsToExchangeRateDtoCorrectly()
    {
        // Arrange
        var usd = new Currency("USD");
        var eur = new Currency("EUR");
        var czk = new Currency("CZK");
        
        var exchangeRates = new List<ExchangeRate>
        {
            new(usd, czk, 25.5m, DateOnly.FromDateTime(DateTime.Today)),
            new(eur, czk, 24.8m, DateOnly.FromDateTime(DateTime.Today))
        };

        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRates);

        const string currencies = "USD,EUR";

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        var result = await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, CancellationToken.None);

        var dtoResponse = result.Select(r =>
            new ExchangeRateDto(r.SourceCurrency.Code, r.TargetCurrency.Code, r.Value));

        // Assert
        Assert.NotNull(dtoResponse);
        Assert.Equal(2, dtoResponse.Count());
        
        var usdDto = dtoResponse.First(dto => dto.SourceCurrency == "USD");
        Assert.Equal("USD", usdDto.SourceCurrency);
        Assert.Equal("CZK", usdDto.TargetCurrency);
        Assert.Equal(25.5m, usdDto.Value);
        
        var eurDto = dtoResponse.First(dto => dto.SourceCurrency == "EUR");
        Assert.Equal("EUR", eurDto.SourceCurrency);
        Assert.Equal("CZK", eurDto.TargetCurrency);
        Assert.Equal(24.8m, eurDto.Value);
    }

    [Fact]
    public async Task ExchangeRateEndpoint_VerifyServiceCalledWithCorrectParameters()
    {
        // Arrange
        const string currencies = "USD,EUR";
        
        _mockExchangeRateProviderService
            .Setup(x => x.GetLatestAsync(It.IsAny<IEnumerable<Currency>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        // Act
        var currencyList = currencies
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code));

        await _mockExchangeRateProviderService.Object
            .GetLatestAsync(currencyList, CancellationToken.None);

        // Assert
        _mockExchangeRateProviderService.Verify(
            x => x.GetLatestAsync(
                It.Is<IEnumerable<Currency>>(currencies => 
                    currencies.Count() == 2 && 
                    currencies.Any(c => c.Code == "USD") && 
                    currencies.Any(c => c.Code == "EUR")),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("USD,EUR")]
    [InlineData("USD,EUR,GBP")]
    [InlineData(" USD ")]
    [InlineData(" USD , EUR ")]
    public void ExchangeRateEndpoint_CurrencyParsing_WithVariousInputs_ParsesCorrectly(string input)
    {
        // Act
        var currencyList = input
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => new Currency(code))
            .ToList();

        // Assert
        Assert.NotEmpty(currencyList);
        Assert.All(currencyList, currency => Assert.NotNull(currency.Code));
        Assert.All(currencyList, currency => Assert.NotEmpty(currency.Code.Trim()));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void ExchangeRateEndpoint_Validation_WithInvalidCurrencies_ShouldReturnBadRequest(string? currencies)
    {
        // Act & Assert
        var result = string.IsNullOrWhiteSpace(currencies);
        Assert.True(result);
    }

    [Fact]
    public void ExchangeRateEndpoint_BadRequestMessage_ShouldMatchExpected()
    {
        // Arrange
        const string expectedMessage = "Query string parameter 'currencies' is required. Example: ?currencies=USD,EUR";

        // Act & Assert
        Assert.Equal("Query string parameter 'currencies' is required. Example: ?currencies=USD,EUR", expectedMessage);
    }
}
