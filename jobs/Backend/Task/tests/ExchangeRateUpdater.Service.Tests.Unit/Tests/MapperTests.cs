using System.Globalization;
using ExchangeRateUpdater.Models.Entities;
using ExchangeRateUpdater.Service.Cnb.Mappers;

namespace ExchangeRateUpdater.Service.Tests.Unit.Tests;

public class MapperTests
{
    private const string DefaultCurrency = "CZK";
    private const string MappingDelimiter = "|";
    private const string MappingDecimalSeparator = ".";
    private const string ColumnInfo = "Country|Currency|Amount|Code|Rate";

    [Test]
    public void GivenCorrectData_ShouldMapAllPropertiesCorrectly()
    {
        var correctData = new List<string>
        {
            "Australia|dollar|1|AUD|16.415",
            "Brazil|real|1|BRL|4.568"
        };

        var mapper = new CnbServiceMapper(DefaultCurrency, MappingDelimiter, MappingDecimalSeparator, throwExceptionOnError: true);

        List<ExchangeRate> result = mapper.Map(ColumnInfo, correctData);

        result[0].Value.Should().Be(16.415m);
        result[0].SourceCurrency.ToString().Should().Be("AUD");
        result[0].TargetCurrency.ToString().Should().Be("CZK");
        result[0].ToString().Should().Be($"AUD/CZK={16.415m.ToString(CultureInfo.CurrentCulture)}");

        result[1].Value.Should().Be(4.568m);
        result[1].SourceCurrency.ToString().Should().Be("BRL");
        result[1].TargetCurrency.ToString().Should().Be("CZK");
        result[1].ToString().Should().Be($"BRL/CZK={4.568m.ToString(CultureInfo.CurrentCulture)}");
    }

    [Test]
    public void GivenIncorrectData_WhenThrowExceptionOnErrorsIsTrue_ShouldThrowException()
    {
        var incorrectData = new List<string> { "Australia||1||16.415" };

        var mapper = new CnbServiceMapper(DefaultCurrency, MappingDelimiter, MappingDecimalSeparator, throwExceptionOnError: true);

        Action act = () => mapper.Map(ColumnInfo, incorrectData);
        act.Should().Throw<Exception>("throwExceptionOnError parameter is true");
    }

    [Test]
    public void GivenIncorrectData_WhenThrowExceptionOnErrorsIsFalse_ShouldNotThrowException()
    {
        var incorrectData = new List<string> 
        { 
            "Australia||1||",
            "Brazil|real|1|BRL|4.568"
        };

        var mapper = new CnbServiceMapper(DefaultCurrency, MappingDelimiter, MappingDecimalSeparator, throwExceptionOnError: false);

        List<ExchangeRate> result = mapper.Map(ColumnInfo, incorrectData);

        result.Single().Value.Should().Be(4.568m);
        result.Single().SourceCurrency.ToString().Should().Be("BRL");
        result.Single().TargetCurrency.ToString().Should().Be("CZK");
        result.Single().ToString().Should().Be($"BRL/CZK={4.568m.ToString(CultureInfo.CurrentCulture)}");
    }
}