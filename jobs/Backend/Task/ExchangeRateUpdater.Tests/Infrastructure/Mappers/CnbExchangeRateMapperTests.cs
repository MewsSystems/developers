using ExchangeRateUpdater.Domain.DTOs;
using ExchangeRateUpdater.Domain.Options;
using ExchangeRateUpdater.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.Infrastructure.Mappers;

 [TestFixture]
public class CnbExchangeRateMapperTests
{
    private Mock<ILogger<CnbExchangeRateMapper>> _loggerMock;
    private IOptions<CurrencyOptions> _currencyOptions;
    private CnbExchangeRateMapper _mapper;

    [SetUp]
    public void SetUp()
    {
        _loggerMock = new Mock<ILogger<CnbExchangeRateMapper>>();

        _currencyOptions = Options.Create(new CurrencyOptions
        {
            BaseCurrency = "CZK"
        });

        _mapper = new CnbExchangeRateMapper(_currencyOptions, _loggerMock.Object);
    }

    [Test]
    public void Map_WhenAllRatesAreToday_NoWarningLogged()
    {
        var today = DateTime.Today.ToString("yyyy-MM-dd");
        var dto = new CnbRateDto
        {
            ExchangeRateDtos = new List<CnbExchangeRateDto>
            {
                new() { CurrencyCode = "USD", ValidFor = today, Amount = 1, Rate = 22.5m },
                new() { CurrencyCode = "EUR", ValidFor = today, Amount = 1, Rate = 25.1m }
            }
        };

        var result = _mapper.Map(dto);

        Assert.That(result, Is.Not.Null);
        _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    [Test]
    public void Map_WhenRatesAreFromYesterday_WarningIsLogged()
    {
        var yesterday = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
        var dto = new CnbRateDto
        {
            ExchangeRateDtos = new List<CnbExchangeRateDto>
            {
                new() { CurrencyCode = "USD", ValidFor = yesterday, Amount = 1, Rate = 22.5m },
                new() { CurrencyCode = "EUR", ValidFor = yesterday, Amount = 1, Rate = 25.1m }
            }
        };

        var result = _mapper.Map(dto);

        Assert.That(result, Is.Not.Null);
        _loggerMock.Verify(x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exchange rates are from date")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
}