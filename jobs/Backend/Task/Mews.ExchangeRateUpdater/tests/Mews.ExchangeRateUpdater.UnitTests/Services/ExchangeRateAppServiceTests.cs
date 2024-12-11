using AutoMapper;
using Mews.ExchangeRateUpdater.Application.ExchangeRates;
using Mews.ExchangeRateUpdater.Application.ExchangeRates.Dto;
using Mews.ExchangeRateUpdater.Application.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Entities.ExchangeRateAgg;

namespace Mews.ExchangeRateUpdater.UnitTests.Services;

/// <summary>
/// Tests the functionality of the ExchangeRateAppService.
/// </summary>
[TestFixture]
public class ExchangeRateAppServiceTests
{
    private Mock<IMapper> _mockMapper = null!;
    private Mock<IExchangeRateRepository> _mockExchangeRateRepository = null!;
    private ExchangeRateAppService _exchangeRateAppService = null!;

    /// <summary>
    /// Initializes mock objects and the ExchangeRateAppService instance before each test is run.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _mockMapper = new Mock<IMapper>();
        _mockExchangeRateRepository = new Mock<IExchangeRateRepository>();
        _exchangeRateAppService = new ExchangeRateAppService(_mockMapper.Object, _mockExchangeRateRepository.Object);
    }

    /// <summary>
    /// Ensures that when the list of currencies is null, the GetTodayExchangeRatesAsync method retrieves all available exchange rates and maps them correctly.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenCurrenciesIsNull_ShouldReturnAllRates()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 23.048m),
            new(new Currency("EUR"), new Currency("CZK"), 25.75m),
            new(new Currency("CAD"), new Currency("CZK"), 17.081m),
            new(new Currency("NZD"), new Currency("CZK"), 15.051m)
        };
        var exchangeRatesDto = new List<ExchangeRateDto>
        {
            new() { SourceCurrency = new CurrencyDto { Code = "USD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 23.048m },
            new() { SourceCurrency = new CurrencyDto { Code = "EUR" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 25.75m },
            new() { SourceCurrency = new CurrencyDto { Code = "CAD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 17.081m },
            new() { SourceCurrency = new CurrencyDto { Code = "NZD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 15.051m }
        };
        var expectedDtos = new List<ExchangeRateDto>
        {
            new() { SourceCurrency = new CurrencyDto { Code = "USD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 23.048m },
            new() { SourceCurrency = new CurrencyDto { Code = "EUR" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 25.75m },
            new() { SourceCurrency = new CurrencyDto { Code = "CAD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 17.081m },
            new() { SourceCurrency = new CurrencyDto { Code = "NZD" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 15.051m }
        };

        _mockExchangeRateRepository.Setup(repo => repo.GetCachedTodayExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates))
            .Returns(exchangeRatesDto);

        // Act
        var result = await _exchangeRateAppService.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.EqualTo(expectedDtos));
        _mockExchangeRateRepository.Verify(repo => repo.GetCachedTodayExchangeRatesAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ExchangeRateDto>>(exchangeRates), Times.Once);
    }

    /// <summary>
    /// Validates that when a list of currencies is provided, the GetTodayExchangeRatesAsync method only returns the exchange rates corresponding to those currencies.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenCurrenciesIsNotNull_ShouldReturnFilteredRates()
    {
        // Arrange
        var exchangeRates = new List<ExchangeRate>
        {
            new(new Currency("USD"), new Currency("CZK"), 23.048m),
            new(new Currency("EUR"), new Currency("CZK"), 25.75m),
            new(new Currency("CAD"), new Currency("CZK"), 17.081m),
            new(new Currency("NZD"), new Currency("CZK"), 15.051m)
        };
        var currencies = new List<Currency> { new("USD"), new("EUR") };
        var expectedDto = new List<ExchangeRateDto>
        {
            new()
            {
                SourceCurrency = new CurrencyDto { Code = "USD" }, TargetCurrency = new CurrencyDto { Code = "CZK" },
                Value = 23.048m
            },
            new()
            {
                SourceCurrency = new CurrencyDto { Code = "EUR" }, TargetCurrency = new CurrencyDto { Code = "CZK" },
                Value = 25.75m
            }
        };

        _mockExchangeRateRepository.Setup(repo => repo.GetCachedTodayExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<ExchangeRateDto>>(It.IsAny<IEnumerable<ExchangeRate>>()))
            .Returns(expectedDto);

        // Act
        var result = await _exchangeRateAppService.GetTodayExchangeRatesAsync(currencies);

        // Assert
        Assert.That(result, Is.EqualTo(expectedDto));
        _mockExchangeRateRepository.Verify(repo => repo.GetCachedTodayExchangeRatesAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<ExchangeRateDto>>(It.IsAny<IEnumerable<ExchangeRate>>()),
            Times.Once);
    }

    /// <summary>
    /// Ensures that when no exchange rates are available, the GetTodayExchangeRatesAsync method returns an empty list.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenExchangeRatesAreEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        _mockExchangeRateRepository.Setup(repo => repo.GetCachedTodayExchangeRatesAsync())
            .ReturnsAsync(new List<ExchangeRate>());

        // Act
        var result = await _exchangeRateAppService.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.Empty);
        _mockExchangeRateRepository.Verify(repo => repo.GetCachedTodayExchangeRatesAsync(), Times.Once);
    }
}