namespace ExchangeRateProvider.Application.Tests.Services;

using Interfaces;
using Application.Services;
using Domain.Entities;
using FluentAssertions;
using Moq;

[TestFixture]
public class ExchangeRateServiceTests
{
    [SetUp]
    public void Setup()
    {
        _exchangeRateProviderMock = new Mock<IExchangeRateProvider>();

        _exchangeRateService = new ExchangeRateService(_exchangeRateProviderMock.Object);
    }

    private ExchangeRateService _exchangeRateService;
    private Mock<IExchangeRateProvider> _exchangeRateProviderMock;

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnValidRates_WhenCurrenciesAreAvailable()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5),
            new(new Currency("EUR"), new Currency("CZK"), 25.3)
        };

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);

        var requestedCurrencies = new[] { "USD", "EUR", "CZK" };

        // Act
        var result = await _exchangeRateService.GetExchangeRatesAsync(requestedCurrencies);

        // Assert
        result.Should().NotBeNull();
        result.Rates.Should().HaveCount(2);
        result.CurrenciesNotResolved.Should().BeEmpty();
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldReturnInvalidCurrencies_WhenNotAvailable()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5)
        };

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);

        var requestedCurrencies = new[] { "EUR", "GBP", "USD" };

        // Act
        var result = await _exchangeRateService.GetExchangeRatesAsync(requestedCurrencies);

        // Assert
        result.Should().NotBeNull();
        result.Rates.Should().HaveCount(1);
        result.CurrenciesNotResolved.Should().Contain(new[] { "EUR", "GBP" });
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldThrowArgumentNullException_WhenRequestedCurrenciesAreNullAsync()
    {
        // Act
        var act = async () => await _exchangeRateService.GetExchangeRatesAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Test]
    public async Task GetExchangeRatesAsync_ShouldIgnoreLocalCurrency()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 22.5)
        };

        _exchangeRateProviderMock.Setup(x => x.GetExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);

        var requestedCurrencies = new[] { "CZK", "USD" };

        // Act
        var result = await _exchangeRateService.GetExchangeRatesAsync(requestedCurrencies);

        // Assert
        result.Should().NotBeNull();
        result.Rates.Should().HaveCount(1);
        result.Rates.Should().ContainSingle(x => x.SourceCurrency.Code == "USD");
        result.CurrenciesNotResolved.Should().BeEmpty();
    }
}
