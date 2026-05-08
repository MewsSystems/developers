using ExchangeRateUpdater.Domain.Models;
using ExchangeRateUpdater.Domain.Services.Implementations;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Tests.Services;

public class XmlExchangeRatesParserTests
{
    private const string ExchangeRatesCorrectFileXmlName = "ExchangeRatesCorrectFile.xml";
    private const string ExchangeRatesIncorrectFormatName = "ExchangeRatesIncorrectFormat.xml";
    private readonly XmlExchangeRatesParser _parser;

    public XmlExchangeRatesParserTests()
    {
        var logger = Substitute.For<ILogger<XmlExchangeRatesParser>>();
        _parser = new XmlExchangeRatesParser(logger);
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnCorrectRates_WhenXmlIsValid()
    {
        var xml = TestDataHelper.LoadTestData(ExchangeRatesCorrectFileXmlName);

        var rates = await _parser.ParseAsync(xml);

        var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
        exchangeRates.Should().ContainSingle(x => x.TargetCurrency.Code == "EUR" && x.Value == 24.300m);
        exchangeRates.Should().ContainSingle(x => x.TargetCurrency.Code == "USD" && x.Value == 20.563m);
    }

    [Fact]
    public async Task ParseAsync_ShouldReturnEmptyRates_WhenDeserializationOfXmlIsNotPossible()
    {
        var xml = TestDataHelper.LoadTestData(ExchangeRatesIncorrectFormatName);

        var rates = await _parser.ParseAsync(xml);

        var exchangeRates = rates as ExchangeRate[] ?? rates.ToArray();
        exchangeRates.Should().BeEmpty();
        
    }
}
