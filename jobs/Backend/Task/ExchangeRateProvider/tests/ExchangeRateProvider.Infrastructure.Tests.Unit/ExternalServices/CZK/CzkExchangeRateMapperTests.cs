using ExchangeRateProvider.Domain.ValueObjects;
using ExchangeRateProvider.Infrastructure.ExternalServices.CZK;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Testing;
using Shouldly;

namespace ExchangeRateProvider.Infrastructure.Tests.Unit.ExternalServices.CZK;

public class CzkExchangeRateMapperTests
{
    private readonly FakeLogger<CzkExchangeRateMapper> _logger;
    private readonly CzkExchangeRateMapper _mapper;

    public CzkExchangeRateMapperTests()
    {
        _logger = new FakeLogger<CzkExchangeRateMapper>();
        _mapper = new CzkExchangeRateMapper(_logger);
    }

    [Fact]
    public void MapToExchangeRates_WithValidResponse_MapsCorrectly()
    {
        // Arrange
        var response = new CzkExchangeRateResponse(new List<CzkRate>
        {
            new() { Amount = 1, CurrencyCode = "USD", Rate = 23.5m, Country = "USA", Currency = "Dollar", Order = 1, ValidFor = DateOnly.FromDateTime(DateTime.Today) },
            new() { Amount = 1, CurrencyCode = "EUR", Rate = 25.0m, Country = "EU", Currency = "Euro", Order = 2, ValidFor = DateOnly.FromDateTime(DateTime.Today) }
        });
        var baseCurrency = new Currency("CZK");

        // Act
        var result = _mapper.MapToExchangeRates(response, baseCurrency);

        // Assert
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(2);
        result[0].BaseCurrency.Code.ShouldBe("CZK");
        result[0].QuoteCurrency.Code.ShouldBe("USD");
        result[0].Rate.ShouldBe(23.5m / 1m);
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Information);
        logEntry.Message.ShouldContain("Mapped 2 exchange rates from CNB response");
    }

    [Fact]
    public void MapToExchangeRates_WithNullResponse_ReturnsEmptyList()
    {
        // Arrange
        var baseCurrency = new Currency("CZK");

        // Act
        var result = _mapper.MapToExchangeRates(null, baseCurrency);

        // Assert
        result.ShouldBeEmpty();
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Warning);
        logEntry.Message.ShouldContain("Received null or empty response from CNB API");
    }

    [Fact]
    public void MapToExchangeRates_WithNullRates_ReturnsEmptyList()
    {
        // Arrange
        var response = new CzkExchangeRateResponse(null!);
        var baseCurrency = new Currency("CZK");

        // Act
        var result = _mapper.MapToExchangeRates(response, baseCurrency);

        // Assert
        result.ShouldBeEmpty();
        
        var logEntry = _logger.Collector.GetSnapshot().Single();
        logEntry.Level.ShouldBe(LogLevel.Warning);
        logEntry.Message.ShouldContain("Received null or empty response from CNB API");
    }
}
