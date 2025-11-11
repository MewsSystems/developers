using ApplicationLayer.Common.Exceptions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateByProviderAndDate;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;
using ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;
using ApplicationLayer.Queries.ExchangeRates.SearchExchangeRates;
using DomainLayer.Aggregates.ExchangeRateAggregate;
using DomainLayer.Aggregates.ProviderAggregate;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for ExchangeRate query handlers.
/// Tests exchange rate retrieval, searching, history, and currency conversion.
/// </summary>
public class ExchangeRateQueryHandlersTests : TestBase
{
    // Helper method to create currencies
    private (Currency usd, Currency eur, Currency jpy) CreateTestCurrencies()
    {
        var usd = Currency.FromCode("USD", id: 1);
        var eur = Currency.FromCode("EUR", id: 2);
        var jpy = Currency.FromCode("JPY", id: 3);

        return (usd, eur, jpy);
    }

    // Helper method to create a test provider
    private ExchangeRateProvider CreateTestProvider(int id = 1)
    {
        var provider = ExchangeRateProvider.Create("European Central Bank", "ECB", "https://ecb.europa.eu", 1);
        typeof(ExchangeRateProvider).GetProperty("Id")!.SetValue(provider, id);
        return provider;
    }

    #region GetLatestExchangeRateQueryHandler Tests

    [Fact]
    public async Task GetLatestExchangeRate_WithValidCurrencyPair_ShouldReturnLatestRate()
    {
        // Arrange
        var handler = new GetLatestExchangeRateQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetLatestExchangeRateQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new GetLatestExchangeRateQuery("USD", "EUR", null);

        var exchangeRate = ExchangeRate.Create(1, 1, 2, 1, 0.85m, new DateOnly(2025, 11, 6));
        typeof(ExchangeRate).GetProperty("Id")!.SetValue(exchangeRate, 100);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(eur);

        MockExchangeRateRepository
            .Setup(x => x.GetLatestRateAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRate);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(100);
        result.BaseCurrencyCode.Should().Be("USD");
        result.TargetCurrencyCode.Should().Be("EUR");
        result.Rate.Should().Be(0.85m);
        result.Multiplier.Should().Be(1);
        result.ProviderName.Should().Be("European Central Bank");
    }

    [Fact]
    public async Task GetLatestExchangeRate_WhenSourceCurrencyNotFound_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetLatestExchangeRateQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetLatestExchangeRateQueryHandler>().Object);

        var query = new GetLatestExchangeRateQuery("XXX", "EUR", null);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("XXX", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetLatestExchangeRate_WhenNoRateFound_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetLatestExchangeRateQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetLatestExchangeRateQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();

        var query = new GetLatestExchangeRateQuery("USD", "EUR", null);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(eur);

        MockExchangeRateRepository
            .Setup(x => x.GetLatestRateAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRate?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region GetExchangeRateByProviderAndDateQueryHandler Tests

    [Fact]
    public async Task GetExchangeRateByProviderAndDate_WithValidInput_ShouldReturnRates()
    {
        // Arrange
        var handler = new GetExchangeRateByProviderAndDateQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetExchangeRateByProviderAndDateQueryHandler>().Object);

        var (usd, eur, jpy) = CreateTestCurrencies();
        var provider = CreateTestProvider();
        var date = new DateOnly(2025, 11, 6);

        var query = new GetExchangeRateByProviderAndDateQuery(1, date);

        var rates = new List<ExchangeRate>
        {
            ExchangeRate.Create(1, 1, 2, 1, 0.85m, date),
            ExchangeRate.Create(1, 1, 3, 100, 11050m, date)
        };

        typeof(ExchangeRate).GetProperty("Id")!.SetValue(rates[0], 100);
        typeof(ExchangeRate).GetProperty("Id")!.SetValue(rates[1], 101);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateAsync(1, date, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { usd, eur, jpy });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(2);
        result.First().BaseCurrencyCode.Should().Be("USD");
        result.First().TargetCurrencyCode.Should().Be("EUR");
        result.Last().TargetCurrencyCode.Should().Be("JPY");
    }

    [Fact]
    public async Task GetExchangeRateByProviderAndDate_WhenProviderNotFound_ShouldReturnEmpty()
    {
        // Arrange
        var handler = new GetExchangeRateByProviderAndDateQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetExchangeRateByProviderAndDateQueryHandler>().Object);

        var query = new GetExchangeRateByProviderAndDateQuery(999, new DateOnly(2025, 11, 6));

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRateProvider?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region GetExchangeRateHistoryQueryHandler Tests

    [Fact]
    public async Task GetExchangeRateHistory_WithValidDateRange_ShouldReturnHistoricalRates()
    {
        // Arrange
        var handler = new GetExchangeRateHistoryQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetExchangeRateHistoryQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new GetExchangeRateHistoryQuery(
            "USD",
            "EUR",
            new DateOnly(2025, 11, 1),
            new DateOnly(2025, 11, 6));

        var rates = new List<ExchangeRate>
        {
            ExchangeRate.Create(1, 1, 2, 1, 0.84m, new DateOnly(2025, 11, 1)),
            ExchangeRate.Create(1, 1, 2, 1, 0.85m, new DateOnly(2025, 11, 2)),
            ExchangeRate.Create(1, 1, 2, 1, 0.86m, new DateOnly(2025, 11, 3))
        };

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(eur);

        MockExchangeRateRepository
            .Setup(x => x.GetHistoryAsync(1, 2, new DateOnly(2025, 11, 1), new DateOnly(2025, 11, 6), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { usd, eur });

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRateProvider> { provider });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().HaveCount(3);
        result.Select(r => r.Rate).Should().ContainInOrder(0.84m, 0.85m, 0.86m);
    }

    [Fact]
    public async Task GetExchangeRateHistory_WhenCurrencyNotFound_ShouldReturnEmpty()
    {
        // Arrange
        var handler = new GetExchangeRateHistoryQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetExchangeRateHistoryQueryHandler>().Object);

        var query = new GetExchangeRateHistoryQuery(
            "XXX",
            "EUR",
            new DateOnly(2025, 11, 1),
            new DateOnly(2025, 11, 6));

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("XXX", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEmpty();
    }

    #endregion

    #region SearchExchangeRatesQueryHandler Tests

    [Fact]
    public async Task SearchExchangeRates_WithCurrencyPairFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var handler = new SearchExchangeRatesQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<SearchExchangeRatesQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new SearchExchangeRatesQuery(
            SourceCurrencyCode: "USD",
            TargetCurrencyCode: "EUR",
            ProviderId: null,
            StartDate: null,
            EndDate: null,
            MinRate: null,
            MaxRate: null,
            PageNumber: 1,
            PageSize: 10);

        var rates = new List<ExchangeRate>
        {
            ExchangeRate.Create(1, 1, 2, 1, 0.85m, new DateOnly(2025, 11, 6)),
            ExchangeRate.Create(1, 1, 2, 1, 0.86m, new DateOnly(2025, 11, 5))
        };

        typeof(ExchangeRate).GetProperty("Id")!.SetValue(rates[0], 100);
        typeof(ExchangeRate).GetProperty("Id")!.SetValue(rates[1], 101);

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRateProvider> { provider });

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { usd, eur });

        MockExchangeRateRepository
            .Setup(x => x.GetHistoryAsync(1, 2, It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(2);
        result.Items.All(r => r.BaseCurrencyCode == "USD").Should().BeTrue();
        result.Items.All(r => r.TargetCurrencyCode == "EUR").Should().BeTrue();
    }

    [Fact]
    public async Task SearchExchangeRates_WithProviderFilter_ShouldReturnRatesFromProvider()
    {
        // Arrange
        var handler = new SearchExchangeRatesQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<SearchExchangeRatesQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new SearchExchangeRatesQuery(
            SourceCurrencyCode: null,
            TargetCurrencyCode: null,
            ProviderId: 1,
            StartDate: null,
            EndDate: null,
            MinRate: null,
            MaxRate: null,
            PageNumber: 1,
            PageSize: 10);

        var rates = new List<ExchangeRate>
        {
            ExchangeRate.Create(1, 1, 2, 1, 0.85m, new DateOnly(2025, 11, 6))
        };

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRateProvider> { provider });

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { usd, eur });

        MockExchangeRateRepository
            .Setup(x => x.GetByProviderAndDateRangeAsync(1, It.IsAny<DateOnly>(), It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().HaveCount(1);
        result.Items.First().ProviderId.Should().Be(1);
    }

    [Fact]
    public async Task SearchExchangeRates_WhenNeitherCurrencyPairNorProviderSpecified_ShouldReturnEmpty()
    {
        // Arrange
        var handler = new SearchExchangeRatesQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<SearchExchangeRatesQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();

        var query = new SearchExchangeRatesQuery(
            SourceCurrencyCode: null,
            TargetCurrencyCode: null,
            ProviderId: null,
            StartDate: null,
            EndDate: null,
            MinRate: null,
            MaxRate: null,
            PageNumber: 1,
            PageSize: 10);

        MockProviderRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ExchangeRateProvider>());

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Currency> { usd, eur });

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    #endregion

    #region ConvertCurrencyQueryHandler Tests

    [Fact]
    public async Task ConvertCurrency_WithValidRate_ShouldReturnConvertedAmount()
    {
        // Arrange
        var handler = new ConvertCurrencyQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ConvertCurrencyQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new ConvertCurrencyQuery(
            SourceCurrencyCode: "USD",
            TargetCurrencyCode: "EUR",
            Amount: 100m,
            ProviderId: null,
            Date: null);

        var exchangeRate = ExchangeRate.Create(1, 1, 2, 1, 0.85m, new DateOnly(2025, 11, 6));

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(eur);

        MockExchangeRateRepository
            .Setup(x => x.GetLatestRateAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRate);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.SourceAmount.Should().Be(100m);
        result.TargetAmount.Should().Be(85.00m); // 100 * 0.85 / 1
        result.SourceCurrencyCode.Should().Be("USD");
        result.TargetCurrencyCode.Should().Be("EUR");
    }

    [Fact]
    public async Task ConvertCurrency_WithMultiplier_ShouldCalculateCorrectly()
    {
        // Arrange
        var handler = new ConvertCurrencyQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ConvertCurrencyQueryHandler>().Object);

        var (usd, _, jpy) = CreateTestCurrencies();
        var provider = CreateTestProvider();

        var query = new ConvertCurrencyQuery(
            SourceCurrencyCode: "USD",
            TargetCurrencyCode: "JPY",
            Amount: 100m,
            ProviderId: null,
            Date: null);

        var exchangeRate = ExchangeRate.Create(1, 1, 3, 100, 11050m, new DateOnly(2025, 11, 6));

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("JPY", It.IsAny<CancellationToken>()))
            .ReturnsAsync(jpy);

        MockExchangeRateRepository
            .Setup(x => x.GetLatestRateAsync(1, 3, It.IsAny<CancellationToken>()))
            .ReturnsAsync(exchangeRate);

        MockProviderRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(provider);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.TargetAmount.Should().Be(11050m); // 100 * 11050 / 100
        result.Multiplier.Should().Be(100);
    }

    [Fact]
    public async Task ConvertCurrency_WhenNoRateFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new ConvertCurrencyQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ConvertCurrencyQueryHandler>().Object);

        var (usd, eur, _) = CreateTestCurrencies();

        var query = new ConvertCurrencyQuery(
            SourceCurrencyCode: "USD",
            TargetCurrencyCode: "EUR",
            Amount: 100m,
            ProviderId: null,
            Date: null);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(usd);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", It.IsAny<CancellationToken>()))
            .ReturnsAsync(eur);

        MockExchangeRateRepository
            .Setup(x => x.GetLatestRateAsync(1, 2, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ExchangeRate?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task ConvertCurrency_WhenSourceCurrencyNotFound_ShouldThrowNotFoundException()
    {
        // Arrange
        var handler = new ConvertCurrencyQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<ConvertCurrencyQueryHandler>().Object);

        var query = new ConvertCurrencyQuery(
            SourceCurrencyCode: "XXX",
            TargetCurrencyCode: "EUR",
            Amount: 100m,
            ProviderId: null,
            Date: null);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("XXX", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(async () =>
            await handler.Handle(query, CancellationToken.None));
    }

    #endregion
}
