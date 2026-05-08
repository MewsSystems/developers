using System.Linq;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class CnbXmlParserTests
{
    [Test]
    public void Parse_ValidXml_ReturnsCorrectExchangeRates()
    {
        // Arrange
        var parser = new CnbXmlParser();
        var baseCurrency = new Currency("CZK");

        // Act
        var rates = parser.Parse(TestData.CnbExchangeRateXml, baseCurrency).ToList();

        // Assert

        Assert.That(rates.Count, Is.EqualTo(31));

        var audRate = rates.FirstOrDefault(r => r.TargetCurrency.Code == "AUD");
        Assert.That(audRate.SourceCurrency.Code, Is.EqualTo("CZK"));
        Assert.That(audRate.Value, Is.EqualTo(13.862m));
    }
    [Test]
    public void Parse_InvalidXml_ReturnsCorrectExchangeRates()
    {
        // Arrange
        var parser = new CnbXmlParser();
        var baseCurrency = new Currency("CZK");

        // Act
        // Assert
        Assert.Throws<ParsingException>(() => parser.Parse(TestData.InvalidCnbExchangeRateXml, baseCurrency));


    }   
}