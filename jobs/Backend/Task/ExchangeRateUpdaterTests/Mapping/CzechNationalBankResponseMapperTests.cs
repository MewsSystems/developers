using System;
using ExchangeRateUpdater.Mapping;
using ExchangeRateUpdater.Models.Configuration;
using ExchangeRateUpdater.Models.CzechNationalBankResponse;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace ExchangeRateUpdaterTests.Mapping;

public class CzechNationalBankResponseMapperTests
{
    private readonly CzechNationalBankResponseMapper _sut;

    public CzechNationalBankResponseMapperTests()
    {
        var optionsMock = new Mock<IOptions<ExchangeRateConfigurationOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new ExchangeRateConfigurationOptions
        {
            SourceCurrency = "CZK"
        });
        _sut = new CzechNationalBankResponseMapper(optionsMock.Object);
    }

    [Fact]
    public void Given_RecordedRate_When_MapToExchangeRate_Then_ReturnsCorrectExchangeRate()
    {
        var recordedRate = new RecordedRate(
            Amount: 1,
            Country: "USA",
            Currency: "Dollar",
            CurrencyCode: "USD",
            Order: 1,
            Rate: 25.1236m,
            ValidFor: DateTime.Now);

        var result = _sut.MapToExchangeRate(recordedRate);

        result.SourceCurrency.Code.Should().Be("CZK");
        result.TargetCurrency.Code.Should().Be("USD");
        result.Value.Should().Be(25.124m);
    }
}
