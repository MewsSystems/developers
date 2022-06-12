using Mews.BackendDeveloperTask.ExchangeRates.Cnb;
using NUnit.Framework;

namespace Mews.BackendDeveloperTask.ExchangeRates;

public class CnbTextExchangeRateParserTests
{
    [Test]
    public void ParsesExchangeRatesFromCnbFile()
    {
        // Arrange
        var file = @"
10 Jun 2022 #113
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.642
Brazil|real|1|BRL|4.780
Bulgaria|lev|1|BGN|12.631
Canada|dollar|1|CAD|18.322
China|renminbi|1|CNY|3.488
Philippines|peso|100|PHP|44.036";
        var parser = new CnbTextExchangeRateParser();

        // Act
        var result = parser.Parse(file);

        // Assert
        var expected = new[] {
            new ExchangeRate(Currency.AUD, Currency.CZK, 16.642m),
            new ExchangeRate(Currency.BRL, Currency.CZK, 4.780m),
            new ExchangeRate(Currency.BGN, Currency.CZK, 12.631m),
            new ExchangeRate(Currency.CAD, Currency.CZK, 18.322m),
            new ExchangeRate(Currency.CNY, Currency.CZK, 3.488m),
            new ExchangeRate(Currency.PHP, Currency.CZK, 0.44036m)
        };

        Assert.That(result, Is.EquivalentTo(expected));
    }
}