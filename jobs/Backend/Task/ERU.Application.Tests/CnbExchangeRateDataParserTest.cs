using ERU.Application.DTOs;
using ERU.Application.Exceptions;
using ERU.Application.Services.ExchangeRate;
using Microsoft.Extensions.Options;

namespace ERU.Application.Tests;

[TestFixture]
[Category("Unit")]
[Parallelizable(ParallelScope.All)]
public class CnbExchangeRateDataParserTest
{
    private Mock<IOptions<ConnectorSettings>> _connectorSettingsConfiguration = null!;
    private CnbExchangeRateDataParser _parser = null!;

    [SetUp]
    public void SetUp()
    {
        _connectorSettingsConfiguration = new Mock<IOptions<ConnectorSettings>>();
        _connectorSettingsConfiguration.SetupGet(x => x.Value).Returns(new ConnectorSettings
        {
            DataSkipLines = 2,
            AmountIndex = 2,
            CodeIndex = 3,
            RateIndex = 4,
        });
        _parser = new CnbExchangeRateDataParser(_connectorSettingsConfiguration.Object);
    }

    [Test]
    public void Parse_CorrectlyParsesTheCnbExchangeRateFileFormat()
    {
        string input = string.Join("\n",
            "07 Oct 2022 #195",
            "Country|Currency|Amount|Code|Rate",
            "Australia|dollar|1|AUD|16.446",
            "Hungary|forint|100|HUF|6.063",
            "Indonesia|rupiah|1000|IDR|1.646"
        );

        var parsed = _parser.Parse(input);

        parsed.Should().BeEquivalentTo(new[]
        {
            new CnbExchangeRateResponse( 1,"AUD",  16.446m),
            new CnbExchangeRateResponse( 100,"HUF",  6.063m),
            new CnbExchangeRateResponse( 1000,"IDR",  1.646m),
        }, options => options.WithStrictOrdering());
    }
    
    [Test]
    [TestCase("")]
    [TestCase("Australia|dollar|1|AUD|16.446")]
    public void Parse_IncorrectInputThrowParseException(string input)
    {
        Action act = () => _parser.Parse(input);
        act.Should().Throw<EmptyParseResultException>();
    }
    
    [Test]
    [TestCase(null)]
    public void Parse_IncorrectInputThrowParameterException(string input)
    {
        Action act = () => _parser.Parse(input);
        act.Should().Throw<ArgumentNullException>();
    }
}