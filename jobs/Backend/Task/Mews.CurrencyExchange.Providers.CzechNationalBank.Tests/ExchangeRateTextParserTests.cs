using FluentAssertions;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client;

namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Tests
{
    public class Tests
    {       
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Split_TextResponse_Into_Lines_Should_Return_CorrectNumberOfLines()
        {            
            var responseText = @"04 Oct 2022 #192
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.027
Brazil|real|1|BRL|4.854
Bulgaria|lev|1|BGN|12.548
Canada|dollar|1|CAD|18.180";
            var lines = ExchangeRateTextParser.SplitTextResponseIntoLines(responseText);
            lines.Should().HaveCount(6);
        }

        [Test]
        public void Parse_TextResponseLine_Should_Return_CorrectData()
        {
            var responseLine = @"Australia|dollar|1|AUD|16.027";
            var providerExchangeRate = ExchangeRateTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeEquivalentTo(new ProviderExchangeRate("AUD", 16.027m));
        }

        [Test]
        public void Parse_TextResponseDateLine_Should_Return_Null()
        {
            var responseLine = @"04 Oct 2022 #192";
            var providerExchangeRate = ExchangeRateTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }

        [Test]
        public void Parse_TextResponseHeaderLine_Should_Return_Null()
        {
            var responseLine = @"Country|Currency|Amount|Code|Rate";
            var providerExchangeRate = ExchangeRateTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }

        [Test]
        public void Parse_TextResponseEmptyLine_Should_Return_Null()
        {
            var responseLine = @"";
            var providerExchangeRate = ExchangeRateTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }
    }
}