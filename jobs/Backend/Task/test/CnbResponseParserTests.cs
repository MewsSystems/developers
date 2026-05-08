using ExchangeRateUpdater;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests;

public class CnbResponseParserTests
{
    private Mock<ILogger<CnbResponseParser>> CreateLoggerMock()
    {
        return new Mock<ILogger<CnbResponseParser>>();
    }

    private ICnbResponseParser CreateParser()
    {
        return new CnbResponseParser(CreateLoggerMock().Object);
    }

    [Fact]
    public void Parse_ValidXml_ReturnsRates()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kurzy datum=""{requestedDate:dd.MM.yyyy}"">
    <tabulka>
        <radek kod=""USD"" mena=""dollar"" mnozstvi=""1"" kurz=""21,048"" zeme=""USA""/>
        <radek kod=""EUR"" mena=""euro"" mnozstvi=""1"" kurz=""22,345"" zeme=""EMU""/>
    </tabulka>
</kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.True(result.ContainsKey("USD"));
        Assert.True(result.ContainsKey("EUR"));
        Assert.Equal(21.048m, result["USD"]);
        Assert.Equal(22.345m, result["EUR"]);
    }

    [Fact]
    public void Parse_EmptyXml_ReturnsEmptyDictionary()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy datum=""{requestedDate:dd.MM.yyyy}""><tabulka></tabulka></kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Parse_DateMismatch_StillReturnsRates()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var responseDate = requestedDate.AddDays(-1);
        var xmlContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kurzy datum=""{responseDate:dd.MM.yyyy}"">
    <tabulka>
        <radek kod=""USD"" mena=""dollar"" mnozstvi=""1"" kurz=""21,048"" zeme=""USA""/>
    </tabulka>
</kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
    }

    [Fact]
    public void Parse_InvalidRootElement_ThrowsException()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?><invalid-root></invalid-root>";

        // Act & Assert
        Assert.Throws<CnbApiException>(() => parser.Parse(xmlContent, requestedDate));
    }

    [Fact]
    public void Parse_InvalidXml_ThrowsException()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = "This is not valid XML";

        // Act & Assert
        Assert.Throws<CnbApiException>(() => parser.Parse(xmlContent, requestedDate));
    }

    [Fact]
    public void Parse_NullOrWhiteSpace_ReturnsEmptyDictionary()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;

        // Act
        var result1 = parser.Parse(null!, requestedDate);
        var result2 = parser.Parse("", requestedDate);
        var result3 = parser.Parse("   ", requestedDate);

        // Assert
        Assert.Empty(result1);
        Assert.Empty(result2);
        Assert.Empty(result3);
    }

    [Fact]
    public void Parse_CurrencyWithAmount_CalculatesCorrectRate()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        // JPY comes as per 100, so rate should be divided by 100
        var xmlContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kurzy datum=""{requestedDate:dd.MM.yyyy}"">
    <tabulka>
        <radek kod=""JPY"" mena=""yen"" mnozstvi=""100"" kurz=""13,425"" zeme=""Japonsko""/>
    </tabulka>
</kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Single(result);
        Assert.Equal(0.13425m, result["JPY"]); // 13.425 / 100
    }

    [Fact]
    public void Parse_MissingDateAttribute_StillReturnsRates()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<kurzy>
    <tabulka>
        <radek kod=""USD"" mena=""dollar"" mnozstvi=""1"" kurz=""21,048"" zeme=""USA""/>
    </tabulka>
</kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
    }

    [Fact]
    public void Parse_MissingRequiredAttributes_SkipsInvalidRows()
    {
        // Arrange
        var parser = CreateParser();
        var requestedDate = DateTime.Today;
        var xmlContent = $@"<?xml version=""1.0"" encoding=""UTF-8""?>
<kurzy datum=""{requestedDate:dd.MM.yyyy}"">
    <tabulka>
        <radek kod=""USD"" mnozstvi=""1"" kurz=""21,048""/>
        <radek kod="""" mnozstvi=""1"" kurz=""22,345""/>
        <radek kod=""EUR"" mnozstvi="""" kurz=""22,345""/>
        <radek kod=""GBP"" mnozstvi=""1"" kurz=""""/>
    </tabulka>
</kurzy>";

        // Act
        var result = parser.Parse(xmlContent, requestedDate);

        // Assert
        Assert.Single(result);
        Assert.True(result.ContainsKey("USD"));
        Assert.False(result.ContainsKey("EUR"));
        Assert.False(result.ContainsKey("GBP"));
    }
}


