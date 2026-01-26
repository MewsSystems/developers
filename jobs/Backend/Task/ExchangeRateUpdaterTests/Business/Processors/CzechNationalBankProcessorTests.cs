using ExchangeRateUpdater.Business.Processors;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Mapping;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Models.CzechNationalBank;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests.Business.Processors;

public class CzechNationalBankProcessorTests
{
    private readonly Mock<IExchangeRateClient<Response>> _clientMock;
    private readonly Mock<IExchangeRateMapper<RecordedRate>> _mapperMock;
    private readonly Mock<ILogger<CzechNationalBankProcessor>> _loggerMock;
    private readonly CzechNationalBankProcessor _processor;

    public CzechNationalBankProcessorTests()
    {
        _clientMock = new Mock<IExchangeRateClient<Response>>();
        _mapperMock = new Mock<IExchangeRateMapper<RecordedRate>>();
        _loggerMock = new Mock<ILogger<CzechNationalBankProcessor>>();
        _processor = new CzechNationalBankProcessor(_clientMock.Object, _mapperMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Given_EmptyClientResponse_When_ProcessExchangeRates_Then_ReturnsFailure()
    {
        _clientMock.Setup(c => c.RetrieveExchangeRatesAsync()).ReturnsAsync(new EmptyResponse());

        var result = await _processor.ProcessExchangeRates(new[] { "USD" });

        result.Success.Should().BeFalse();
        result.Error.Should().Be("CzechNationalBankProcessor_UnexpectedClientError");
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankProcessor_FinishedProcessingCurrencies")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Never);
    }

    [Fact]
    public async Task Given_ValidResponse_When_ProcessExchangeRates_Then_ReturnsMappedRates()
    {
        var usdRate = new RecordedRate(1, "USA", "Dollar", "USD", 1, 25.0m, DateTime.Now);
        var response = new Response(new List<RecordedRate> { usdRate });
        _clientMock.Setup(c => c.RetrieveExchangeRatesAsync()).ReturnsAsync(response);
        
        var mappedRate = new ExchangeRate(new Currency("CZK"), new Currency("USD"), 25.0m);
        _mapperMock.Setup(m => m.MapToExchangeRate(usdRate)).Returns(mappedRate);

        var result = await _processor.ProcessExchangeRates(new[] { "USD" });

        result.Success.Should().BeTrue();
        result.Value.FirstOrDefault().Should().Be(mappedRate);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankProcessor_ProcessingCurrencies")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankProcessor_FinishedProcessingCurrencies")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Given_CurrencyNotFound_When_ProcessExchangeRates_Then_LogsWarningAndSkips()
    {
        var eurRate = new RecordedRate(1, "EMU", "Euro", "EUR", 1, 24.0m, DateTime.Now);
        var response = new Response(new List<RecordedRate> { eurRate });
        _clientMock.Setup(c => c.RetrieveExchangeRatesAsync()).ReturnsAsync(response);

        var result = await _processor.ProcessExchangeRates(new[] { "USD" });

        result.Success.Should().BeTrue();
        result.Value.Should().BeEmpty();
        
        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("CzechNationalBankProcessor_CurrencyNotFound_USD")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
