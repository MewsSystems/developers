using ExchangeRateUpdater.Clients.Cnb.Parsers;
using ExchangeRateUpdater.Clients.Cnb.Responses;
using ExchangeRateUpdater.Tests.Shared.Builders;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExchangeRateUpdater.Clients.Cnb.Tests.Parsers;

public class CnbClientResponseParserTests
{
    private readonly CnbClientResponseParser _parser;

    public CnbClientResponseParserTests()
    {
        _parser = new CnbClientResponseParser(new Mock<ILogger<CnbClientResponseParser>>().Object);
    }

    [Theory]
    [MemberData(nameof(MappingTestCases))]
    public void Given_line_when_parsing_line_to_exchange_rate_then_it_should_map_as_expected(string line, ExchangeRateDto expectedResult)
    {
        //Arrange - Act
        var result = _parser.ExtractExchangeRate(line);

        //Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object?[]> MappingTestCases()
    {
        yield return new object?[]
        {
            "Australia|dollar|1|AUD|15.867",
            new ExchangeRateDtoBuilder().WithAmount(1).WithCode("AUD").WithCurrency("dollar").WithCountry("Australia").WithRate((decimal)15.867).Build()
        };
        
        yield return new object?[]
        {
            "Australia|dollar|1|AUD|",
            null
        };
        
        yield return new object?[]
        {
            "Australia|dollar|1|AUD|Test",
            null
        };
    }
}