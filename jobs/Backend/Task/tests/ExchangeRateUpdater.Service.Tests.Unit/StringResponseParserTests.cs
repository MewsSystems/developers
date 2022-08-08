using ExchangeRatesSearcherService.Parser;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Service.Tests.Unit;

[TestFixture]
public class StringResponseParserTests
{
    [Test]
    public void GivenNullOrEmptyStringResponse_ShouldReturnArgumentExceptionWithTheCorrectMessage()
    {
        const string response = "";
        Action act = () => StringResponseParser.Parse(response);

        act.Should()
            .Throw<ArgumentException>()
            .Which.Message.Contains("Response string cannot be null or whitespace");
    }
    
    [Test]
    public void GivenResponseWithoutDateLine_ShouldReturnExceptionWithTheCorrectMessage()
    {
        const string response = "\nCountry|Currency|Amount|Code|Rate";
        Action act = () => StringResponseParser.Parse(response);

        act.Should()
            .Throw<Exception>()
            .Which.Message.Contains("Invalid response message. Date information is missing");
    }
    
    [TestCase("05 Aug 2022 #\nRestOfTheResponse")]
    [TestCase("05Aug2022 #151\nnRestOfTheResponse")]
    [TestCase("5 Aug 2022 #151\nnRestOfTheResponse")]
    [TestCase("05 August 2022 #151\nnRestOfTheResponse")]
    [TestCase("05 08 2022 #151\nnRestOfTheResponse")]
    [TestCase("05 Aug 22 #151\nnRestOfTheResponse")]
    [TestCase("05 Aug 2022\nnRestOfTheResponse")]
    [TestCase("Aug 2022 #151\nnRestOfTheResponse")]
    [TestCase("05 2022 #151\nnRestOfTheResponse")]
    public void GivenResponseWithInvalidDateLine_ShouldReturnExceptionWithTheCorrectMessage(string invalidResponse)
    {
        // [0-9]{2} .* #[0-9]*
        Action act = () => StringResponseParser.Parse(invalidResponse);

        act.Should()
            .Throw<Exception>()
            .Which.Message.Contains("Invalid response message. Date information has incorrect format");
    }
    
    [TestCase("05 Aug 2022 #151\n")]
    [TestCase("05 Aug 2022 #151\n\nAustralia|dollar|1|AUD|16.711")]
    public void GivenResponseWithoutColumnNamesLine_ShouldReturnExceptionWithTheCorrectMessage(string invalidResponse)
    {
        Action act = () => StringResponseParser.Parse(invalidResponse);

        act.Should()
            .Throw<Exception>()
            .Which.Message.Contains("Invalid response message. Column information is missing");
    }
}