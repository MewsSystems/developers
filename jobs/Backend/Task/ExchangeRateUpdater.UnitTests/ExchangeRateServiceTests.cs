using AutoMapper;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates;
using ExchangeRateUpdater.ApplicationServices.ExchangeRates.Dto;
using ExchangeRateUpdater.ApplicationServices.Interfaces;
using ExchangeRateUpdater.Domain;
using NUnit.Framework;
using Moq;

namespace ExchangeRateUpdater.UnitTests;

[TestFixture]
public class ExchangeRateServiceTests
{
    private Mock<IMapper> _mapperMock;
    private Mock<IExchangeRateRepository> _exchangeRateRepository;
    private ExchangeRateService _exchangeRateService;

    [SetUp]
    public void Setup()
    {
        _mapperMock = new Mock<IMapper>();
        _exchangeRateRepository = new Mock<IExchangeRateRepository>();
        _exchangeRateService = new ExchangeRateService(_mapperMock.Object, _exchangeRateRepository.Object);
    }

    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenCurrenciesIsNull_ReturnsAllRates()
    {
        // Arrange
        var exchangeRates = new ExchangeRate[]
        {
            new(new Currency("JPY"), new Currency("CZK"), 0.15499m),
            new(new Currency("EUR"), new Currency("CZK"), 25.020m),
            new(new Currency("TRY"), new Currency("CZK"), 0.68065m)
        };

        var exchangeRatesDto = new ExchangeRateDto[]
        {
            new ExchangeRateDto { SourceCurrency = new CurrencyDto { Code = "JPY" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 0.15499m },
            new ExchangeRateDto { SourceCurrency = new CurrencyDto { Code = "EUR" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 25.020m },
            new ExchangeRateDto { SourceCurrency = new CurrencyDto { Code = "TRY" }, TargetCurrency = new CurrencyDto { Code = "CZK" }, Value = 0.68065m }
        };

        var expectedDtos = exchangeRatesDto.ToList();

        _exchangeRateRepository.Setup(r => r.GetTodayExchangeRatesAsync()).ReturnsAsync(exchangeRates);

        _mapperMock.Setup(m => m.Map<IEnumerable<ExchangeRateDto>>(exchangeRates)).Returns(exchangeRatesDto);

        // Act
        var result = await _exchangeRateService.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.EquivalentTo(expectedDtos));
        _exchangeRateRepository.Verify(r => r.GetTodayExchangeRatesAsync(), Times.Once);
    }

    [Test]
    public async Task GetTodayExchangeRatesAsync_WithCurrencies_ReturnsFilteredRates()
    {
        // Arrange
        var exchangeRates = new ExchangeRate[]
        {
            new(new Currency("JPY"), new Currency("CZK"), 0.15499m),
            new(new Currency("EUR"), new Currency("CZK"), 25.020m),
            new(new Currency("TRY"), new Currency("CZK"), 0.68065m),
        };

        var currencies = new Currency[] { new("JPY"), new("EUR"), new("ABC") };

        var expectedDto = new ExchangeRateDto[]
        {
            new ExchangeRateDto { SourceCurrency = new CurrencyDto { Code = "JPY" }, TargetCurrency = new CurrencyDto { Code = "CZK" },Value = 0.15499m },
            new ExchangeRateDto { SourceCurrency = new CurrencyDto { Code = "EUR" }, TargetCurrency = new CurrencyDto { Code = "CZK" },Value = 25.020m }
        };

        _exchangeRateRepository
            .Setup(r => r.GetTodayExchangeRatesAsync())
            .ReturnsAsync(exchangeRates);

        _mapperMock
            .Setup(m => m.Map<IEnumerable<ExchangeRateDto>>(It.IsAny<IEnumerable<ExchangeRate>>()))
            .Returns(expectedDto);

        // Act
        var result = await _exchangeRateService.GetTodayExchangeRatesAsync(currencies);

        // Assert
        Assert.That(result, Is.EqualTo(expectedDto));
        _exchangeRateRepository.Verify(r => r.GetTodayExchangeRatesAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<IEnumerable<ExchangeRateDto>>(It.IsAny<IEnumerable<ExchangeRate>>()), Times.Once);
    }

    [Test]
    public async Task GetTodayExchangeRatesAsync_WhenRepositoryReturnsEmpty_ShouldReturnEmptyList()
    {
        // Arrange
        _exchangeRateRepository.Setup(r => r.GetTodayExchangeRatesAsync()).ReturnsAsync(Array.Empty<ExchangeRate>());

        // Act
        var result = await _exchangeRateService.GetTodayExchangeRatesAsync();

        // Assert
        Assert.That(result, Is.Empty);
        _exchangeRateRepository.Verify(r => r.GetTodayExchangeRatesAsync(), Times.Once);
    }
}