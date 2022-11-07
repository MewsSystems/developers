using System;
using System.Linq;
using ExchangeRateUpdater.App;
using ExchangeRateUpdater.Domain;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class ExchangeRateParserTest
{
    [Fact]
    public void ParseData_WhenIsEmpty_ShouldReturnEmpty()
    {
        var data = "";

        var result = ExchangeRateParser.ParseExchangeRates(data);

        Assert.Empty(result);
    }

    [Fact]
    public void ParseData_WhenContainsOnlyDate_ShouldReturnEmpty()
    {
        var data = "04.11.2022 #214\nzemě|měna|množství|kód|kurz";

        var result = ExchangeRateParser.ParseExchangeRates(data);

        Assert.Empty(result);
    }

    [Fact]
    public void ParseData_WhenLinesAreEmpty_ShouldReturnEmpty()
    {
        var data = "04.11.2022 #214\nzemě|měna|množství|kód|kurz\n\n\n\n\n";

        var result = ExchangeRateParser.ParseExchangeRates(data);

        Assert.Empty(result);
    }

    [Fact]
    public void ParseData_WhenLineIsInvalid_ShouldReturnOnlyValid()
    {
        var data =
            "04.11.2022 #214\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,950\nBrazílie|1|BRL|4,916";

        var aud = new Currency("AUD");

        var result = ExchangeRateParser.ParseExchangeRates(data).ToList();

        Assert.Single(result);
        Assert.Equal(aud, result[0].TargetCurrency);
        Assert.Equal(15.95m, result[0].Value);
    }

    [Fact]
    public void ParseData_WhenRateIsInvalid_ShouldThrowsException()
    {
        var data =
            "04.11.2022 #214\nzemě|měna|množství|kód|kurz\nAustrálie|dolar|1|AUD|15,ff950";

        Assert.Throws<Exception>(() => ExchangeRateParser.ParseExchangeRates(data));
    }

    [Fact]
    public void ParseData_WhenDataIsValid_ShouldReturnCorrectData()
    {
        var data =
            "04.11.2022 #214\nzemě|měna|množství|kód|kurz\nDánsko|koruna|1|DKK|3,282\nEMU|euro|1|EUR|24,420\n" +
            "Filipíny|peso|100|PHP|42,343";

        var php = new Currency("PHP");

        var result = ExchangeRateParser.ParseExchangeRates(data).ToList();

        Assert.Equal(3, result.Count);
        Assert.Equal(php, result[2].TargetCurrency);
        Assert.Equal(100, result[2].Count);
        Assert.Equal(42.343m, result[2].Value);
        Assert.Equal("100 CZK/PHP=42.343", result[2].ToString());
    }
}