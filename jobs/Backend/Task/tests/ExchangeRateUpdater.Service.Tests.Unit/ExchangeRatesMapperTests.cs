using ExchangeRatesSearcherService.Mapper;
using ExchangeRateUpdater.Service.Tests.Unit.Configurations;
using ExchangeRateUpdater.Service.Tests.Unit.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Service.Tests.Unit;

[TestFixture]
public class ExchangeRatesMapperTests
{
    [Test]
    public void GivenValidResponseLines_ShouldMapLinesToExchangeRateList()
    {
        var invalidResponse = new ExchangeRateResponseStringBuilder()
            .WithDateLine("05 Aug 2022 #151")
            .WithColumnInfo("Country|Currency|Amount|Code|Rate")
            .WithExchangeRateInfo("Australia|dollar|1|AUD|16.711")
            .WithExchangeRateInfo("Bulgaria|lev|1|BGN|12.564")
            .Build();
        
        var sut = CreateSut(invalidResponse);

        var exchangeRates = sut.Map();

        var exchangeRatesList = exchangeRates.ToList();
        exchangeRatesList.Count.Should().Be(2);
        exchangeRatesList[0].ToString().Should().Be("AUD/CZK=16,711");
        exchangeRatesList[1].ToString().Should().Be("BGN/CZK=12,564");
    }
    
    [TestCase("Country|Currency")]
    [TestCase("Country|Currency|Amount")]
    [TestCase("Country|Currency|Amount|Code")]
    [TestCase("Country|Currency|Code|Rate")]
    [TestCase("Country|Currency|Amount|Rate")]
    public void GivenResponseWithInvalidColumnNamesLine_ShouldThroeExceptionWithCorrectMessage(string invalidColumnNamesLine)
    {
        var invalidResponse = new ExchangeRateResponseStringBuilder()
            .WithDateLine("05 Aug 2022 #151")
            .WithColumnInfo(invalidColumnNamesLine)
            .Build();
        
        var sut = CreateSut(invalidResponse);

        Action act = () => sut.Map();

        act.Should()
            .Throw<Exception>()
            .Which.Message.Contains("Incorrect column information format");
    }
    
    [TestCase("Australia")]
    [TestCase("Australia|dollar")]
    [TestCase("Australia|dollar|1|1")]
    [TestCase("Australia|dollar|1|AUD")]
    [TestCase("Australia|dollar||AUD|16.711")]
    [TestCase("Australia|dollar|1||16.711")]
    [TestCase("Australia|dollar|1|AUD|")]
    public void GivenResponseWithTwoExchangeRateLines_OneValidAndAnotherInvalid_ShouldMapCorrectlyTheValidLine_AndSkipTheInvalidOne(string invalidExchangeRateLine)
    {
        var invalidResponse = new ExchangeRateResponseStringBuilder()
            .WithDateLine("05 Aug 2022 #151")
            .WithColumnInfo("Country|Currency|Amount|Code|Rate")
            .WithExchangeRateInfo("Australia|dollar|1|AUD|16.711")
            .WithExchangeRateInfo(invalidExchangeRateLine)
            .Build();
        
        var sut = CreateSut(invalidResponse);

        var exchangeRates = sut.Map();

        exchangeRates.Should().NotBeNull();
        
        var exchangeRatesList = exchangeRates.ToList();
        exchangeRatesList.Count.Should().Be(1);
        exchangeRatesList[0].ToString().Should().Be("AUD/CZK=16,711");
    }
    
    [TestCase("Australia|dollar|SomeString|AUD|16.711")]
    [TestCase("Australia|dollar|1|AUD|SomeString")]
    public void GivenResponseWithTwoExchangeRateLines_OneValidAndAnotherWithInvalidDecimal_ShouldMapCorrectlyTheValidLine_AndSkipTheInvalidOne(string invalidExchangeRateLine)
    {
        var invalidResponse = new ExchangeRateResponseStringBuilder()
            .WithDateLine("05 Aug 2022 #151")
            .WithColumnInfo("Country|Currency|Amount|Code|Rate")
            .WithExchangeRateInfo("Australia|dollar|1|AUD|16.711")
            .WithExchangeRateInfo(invalidExchangeRateLine)
            .Build();
        
        var sut = CreateSut(invalidResponse);

        var exchangeRates = sut.Map();

        exchangeRates.Should().NotBeNull();
        
        var exchangeRatesList = exchangeRates.ToList();
        exchangeRatesList.Count.Should().Be(1);
        exchangeRatesList[0].ToString().Should().Be("AUD/CZK=16,711");
    }

    private static ExchangeRatesMapper CreateSut(IReadOnlyList<string> lines)
    {
        var logger = new StubLogger();
        var settings = new CzechNationalBankApiSettings
        {
            ApiBaseAddress = "someBaseAddres",
            Delimiter = "|",
            DecimalSeparator = "."
        };

        return new ExchangeRatesMapper(lines, logger, settings);
    }
}