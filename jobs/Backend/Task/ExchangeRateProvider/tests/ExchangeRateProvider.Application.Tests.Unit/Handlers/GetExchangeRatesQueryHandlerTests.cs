using ExchangeRateProvider.Application.Handlers;
using ExchangeRateProvider.Application.Queries;
using ExchangeRateProvider.Domain.Interfaces;
using ExchangeRateProvider.Domain.ValueObjects;
using FakeItEasy;
using Shouldly;

namespace ExchangeRateProvider.Application.Tests.Unit.Handlers;

public class GetExchangeRatesQueryHandlerTests
{
    private readonly IExchangeRateServiceFactory _fakeServiceFactory;
    private readonly IExchangeRateService _fakeExchangeRateService;
    private readonly GetExchangeRatesQueryHandler _handler;

    public GetExchangeRatesQueryHandlerTests()
    {
        _fakeServiceFactory = A.Fake<IExchangeRateServiceFactory>();
        _fakeExchangeRateService = A.Fake<IExchangeRateService>();
        _handler = new GetExchangeRatesQueryHandler(_fakeServiceFactory);

        A.CallTo(() => _fakeServiceFactory.GetService(A<Currency>._))
            .Returns(_fakeExchangeRateService);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithValidQuery_ReturnsAllRates(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency);

        var expectedRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(3);
        result[0].BaseCurrency.Code.ShouldBe(baseCurrencyCode);
        result[0].QuoteCurrency.Code.ShouldBe("EUR");
        result[0].Rate.ShouldBe(0.04m);
        result[1].QuoteCurrency.Code.ShouldBe("USD");
        result[2].QuoteCurrency.Code.ShouldBe("GBP");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithNullQuoteCurrencies_ReturnsAllRates(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency, null);

        var expectedRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldAllBe(r => r.BaseCurrency.Code == baseCurrencyCode);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithEmptyQuoteCurrenciesList_ReturnsAllRates(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency, new List<Currency>());

        var expectedRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(expectedRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithSpecificQuoteCurrencies_ReturnsFilteredRates(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency> { new("EUR"), new("GBP") };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m),
            new(new Currency(baseCurrencyCode), new Currency("JPY"), 5.5m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(r => r.QuoteCurrency.Code == "EUR");
        result.ShouldContain(r => r.QuoteCurrency.Code == "GBP");
        result.ShouldNotContain(r => r.QuoteCurrency.Code == "USD");
        result.ShouldNotContain(r => r.QuoteCurrency.Code == "JPY");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithSingleQuoteCurrency_ReturnsOnlyMatchingRate(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency> { new("EUR") };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(1);
        result[0].QuoteCurrency.Code.ShouldBe("EUR");
        result[0].Rate.ShouldBe(0.04m);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithNonMatchingQuoteCurrencies_ReturnsEmptyList(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency> { new("JPY"), new("CHF") };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithCaseInsensitiveQuoteCurrency_MatchesCorrectly(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency> { new("eur"), new("GBP") };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(r => r.QuoteCurrency.Code == "EUR");
        result.ShouldContain(r => r.QuoteCurrency.Code == "GBP");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithMixedCaseQuoteCurrencies_MatchesCorrectly(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency> { new("EuR"), new("uSd") };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(r => r.QuoteCurrency.Code == "EUR");
        result.ShouldContain(r => r.QuoteCurrency.Code == "USD");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WhenServiceReturnsEmptyList_ReturnsEmptyList(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency);

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(new List<ExchangeRate>());

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEmpty();
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("CZK")]
    [InlineData("EUR")]
    public async Task HandleAsync_CallsServiceFactoryWithCorrectBaseCurrency(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency);

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(A<Currency>._, A<CancellationToken>._))
            .Returns(new List<ExchangeRate>());

        // Act
        await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        A.CallTo(() => _fakeServiceFactory.GetService(A<Currency>.That.Matches(c => c.Code == baseCurrencyCode)))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_CallsExchangeRateServiceWithCorrectParameters(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var cancellationToken = new CancellationToken();
        var query = new GetExchangeRatesQuery(baseCurrency);

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(A<Currency>._, A<CancellationToken>._))
            .Returns(new List<ExchangeRate>());

        // Act
        await _handler.HandleAsync(query, cancellationToken);

        // Assert
        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(
            A<Currency>.That.Matches(c => c.Code == baseCurrencyCode),
            cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_MapsExchangeRatesToDtosCorrectly(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency);

        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.040123m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045678m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(exchangeRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);

        // Verify first DTO mapping
        result[0].BaseCurrency.Code.ShouldBe(baseCurrencyCode);
        result[0].QuoteCurrency.Code.ShouldBe("EUR");
        result[0].Rate.ShouldBe(0.040123m);

        // Verify second DTO mapping
        result[1].BaseCurrency.Code.ShouldBe(baseCurrencyCode);
        result[1].QuoteCurrency.Code.ShouldBe("USD");
        result[1].Rate.ShouldBe(0.045678m);
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_WithDuplicateQuoteCurrencies_FiltersCorrectly(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var quoteCurrencies = new List<Currency>
        {
            new("EUR"),
            new("EUR"), // Duplicate
            new("USD")
        };
        var query = new GetExchangeRatesQuery(baseCurrency, quoteCurrencies);

        var allRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), 0.04m),
            new(new Currency(baseCurrencyCode), new Currency("USD"), 0.045m),
            new(new Currency(baseCurrencyCode), new Currency("GBP"), 0.035m)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(allRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result.Count.ShouldBe(2);
        result.ShouldContain(r => r.QuoteCurrency.Code == "EUR");
        result.ShouldContain(r => r.QuoteCurrency.Code == "USD");
    }

    [Theory]
    [InlineData("CZK")]
    public async Task HandleAsync_PreservesDecimalPrecision(string baseCurrencyCode)
    {
        // Arrange
        var baseCurrency = new Currency(baseCurrencyCode);
        var query = new GetExchangeRatesQuery(baseCurrency);

        var precisRate = 0.0401234567890123456789m;
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency(baseCurrencyCode), new Currency("EUR"), precisRate)
        };

        A.CallTo(() => _fakeExchangeRateService.GetExchangeRatesAsync(baseCurrency, A<CancellationToken>._))
            .Returns(exchangeRates);

        // Act
        var result = await _handler.HandleAsync(query, CancellationToken.None);

        // Assert
        result.ShouldNotBeNull();
        result[0].Rate.ShouldBe(precisRate);
    }
}