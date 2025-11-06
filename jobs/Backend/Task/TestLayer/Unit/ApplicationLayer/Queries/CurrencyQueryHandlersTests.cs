using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using ApplicationLayer.Queries.Currencies.GetCurrencyById;
using DomainLayer.ValueObjects;
using FluentAssertions;
using Moq;

namespace Unit.ApplicationLayer.Queries;

/// <summary>
/// Unit tests for Currency query handlers.
/// Tests all three: GetAllCurrencies, GetCurrencyById, GetCurrencyByCode.
/// </summary>
public class CurrencyQueryHandlersTests : TestBase
{
    #region GetAllCurrenciesQueryHandler Tests

    private readonly GetAllCurrenciesQueryHandler _getAllHandler;

    public CurrencyQueryHandlersTests()
    {
        _getAllHandler = new GetAllCurrenciesQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetAllCurrenciesQueryHandler>().Object);
    }

    [Fact]
    public async Task GetAllCurrencies_WithData_ShouldReturnPagedResults()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 10, IncludePagination: true);

        var currencies = new List<Currency>
        {
            Currency.FromCode("USD", id: 1),
            Currency.FromCode("EUR", id: 2),
            Currency.FromCode("GBP", id: 3),
            Currency.FromCode("JPY", id: 4),
            Currency.FromCode("CHF", id: 5)
        };

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)currencies);

        // Act
        var result = await _getAllHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(5);
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(1);
        result.PageSize.Should().Be(10);

        MockCurrencyRepository.Verify(
            x => x.GetAllAsync(It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetAllCurrencies_WithSearchTerm_ShouldFilterResults()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(SearchTerm: "US", IncludePagination: false);

        var currencies = new List<Currency>
        {
            Currency.FromCode("USD", id: 1),  // Contains "US"
            Currency.FromCode("EUR", id: 2),
            Currency.FromCode("GBP", id: 3),
            Currency.FromCode("RUB", id: 4),  // Contains "RU", not "US"
            Currency.FromCode("AUD", id: 5),
            Currency.FromCode("RUS", id: 6)   // Hypothetical code containing "US"
        };

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)currencies);

        // Act
        var result = await _getAllHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2); // USD and RUS contain "US"
        result.Items.Should().Contain(dto => dto.Code == "USD");
        result.Items.Should().Contain(dto => dto.Code == "RUS");
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task GetAllCurrencies_WithPagination_ShouldReturnCorrectPage()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(PageNumber: 2, PageSize: 2, IncludePagination: true);

        var currencies = new List<Currency>
        {
            Currency.FromCode("USD", id: 1),
            Currency.FromCode("EUR", id: 2),
            Currency.FromCode("GBP", id: 3),
            Currency.FromCode("JPY", id: 4),
            Currency.FromCode("CHF", id: 5)
        };

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)currencies);

        // Act
        var result = await _getAllHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(2); // Page 2, size 2: items 3-4
        result.TotalCount.Should().Be(5);
        result.PageNumber.Should().Be(2);
        result.PageSize.Should().Be(2);
        result.Items.Select(i => i.Code).Should().ContainInOrder("GBP", "JPY");
    }

    [Fact]
    public async Task GetAllCurrencies_WithoutPagination_ShouldReturnAllItems()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery(IncludePagination: false);

        var currencies = new List<Currency>
        {
            Currency.FromCode("USD", id: 1),
            Currency.FromCode("EUR", id: 2),
            Currency.FromCode("GBP", id: 3),
            Currency.FromCode("JPY", id: 4),
            Currency.FromCode("CHF", id: 5),
            Currency.FromCode("CAD", id: 6),
            Currency.FromCode("AUD", id: 7),
            Currency.FromCode("NZD", id: 8),
            Currency.FromCode("SEK", id: 9),
            Currency.FromCode("NOK", id: 10),
            Currency.FromCode("DKK", id: 11),
            Currency.FromCode("PLN", id: 12)
        };

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)currencies);

        // Act
        var result = await _getAllHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(12); // All items, no pagination
        result.TotalCount.Should().Be(12);
    }

    [Fact]
    public async Task GetAllCurrencies_WithNoCurrencies_ShouldReturnEmptyResult()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery();

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency>());

        // Act
        var result = await _getAllHandler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().BeEmpty();
        result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAllCurrencies_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var query = new GetAllCurrenciesQuery();
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockCurrencyRepository
            .Setup(x => x.GetAllAsync(token))
            .ReturnsAsync((IEnumerable<Currency>)new List<Currency>());

        // Act
        await _getAllHandler.Handle(query, token);

        // Assert
        MockCurrencyRepository.Verify(
            x => x.GetAllAsync(token),
            Times.Once);
    }

    #endregion

    #region GetCurrencyByIdQueryHandler Tests

    [Fact]
    public async Task GetCurrencyById_WithExistingId_ShouldReturnDto()
    {
        // Arrange
        var handler = new GetCurrencyByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByIdQueryHandler>().Object);

        var query = new GetCurrencyByIdQuery(CurrencyId: 1);
        var currency = Currency.FromCode("USD", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Code.Should().Be("USD");

        MockCurrencyRepository.Verify(
            x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencyById_WithNonExistentId_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetCurrencyByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByIdQueryHandler>().Object);

        var query = new GetCurrencyByIdQuery(CurrencyId: 999);

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        MockCurrencyRepository.Verify(
            x => x.GetByIdAsync(999, It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencyById_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new GetCurrencyByIdQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByIdQueryHandler>().Object);

        var query = new GetCurrencyByIdQuery(CurrencyId: 1);
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockCurrencyRepository
            .Setup(x => x.GetByIdAsync(1, token))
            .ReturnsAsync(Currency.FromCode("USD", id: 1));

        // Act
        await handler.Handle(query, token);

        // Assert
        MockCurrencyRepository.Verify(
            x => x.GetByIdAsync(1, token),
            Times.Once);
    }

    #endregion

    #region GetCurrencyByCodeQueryHandler Tests

    [Fact]
    public async Task GetCurrencyByCode_WithExistingCode_ShouldReturnDto()
    {
        // Arrange
        var handler = new GetCurrencyByCodeQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByCodeQueryHandler>().Object);

        var query = new GetCurrencyByCodeQuery(Code: "USD");
        var currency = Currency.FromCode("USD", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(1);
        result.Code.Should().Be("USD");

        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencyByCode_WithLowercaseCode_ShouldConvertToUppercase()
    {
        // Arrange
        var handler = new GetCurrencyByCodeQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByCodeQueryHandler>().Object);

        var query = new GetCurrencyByCodeQuery(Code: "usd"); // Lowercase input
        var currency = Currency.FromCode("USD", id: 1);

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()))
            .ReturnsAsync(currency);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Code.Should().Be("USD");

        // Verify it was called with uppercase
        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("USD", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencyByCode_WithNonExistentCode_ShouldReturnNull()
    {
        // Arrange
        var handler = new GetCurrencyByCodeQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByCodeQueryHandler>().Object);

        var query = new GetCurrencyByCodeQuery(Code: "XYZ");

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("XYZ", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Currency?)null);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeNull();

        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("XYZ", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetCurrencyByCode_WithCancellationToken_ShouldPassTokenToRepository()
    {
        // Arrange
        var handler = new GetCurrencyByCodeQueryHandler(
            MockUnitOfWork.Object,
            CreateMockLogger<GetCurrencyByCodeQueryHandler>().Object);

        var query = new GetCurrencyByCodeQuery(Code: "EUR");
        var cts = new CancellationTokenSource();
        var token = cts.Token;

        MockCurrencyRepository
            .Setup(x => x.GetByCodeAsync("EUR", token))
            .ReturnsAsync(Currency.FromCode("EUR", id: 2));

        // Act
        await handler.Handle(query, token);

        // Assert
        MockCurrencyRepository.Verify(
            x => x.GetByCodeAsync("EUR", token),
            Times.Once);
    }

    #endregion
}
