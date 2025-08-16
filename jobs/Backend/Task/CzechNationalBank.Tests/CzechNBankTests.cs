using ExchangeRate.Datalayer.Models;
using ExchangeRates.Providers.CzechNationalBank.Provider;
using FluentAssertions;
namespace CzechNationalBank.Tests
{
    /*
     * 10 May 2024 #90
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.285
Brazil|real|1|BRL|4.503
Bulgaria|lev|1|BGN|12.750
Canada|dollar|1|CAD|16.910
China|renminbi|1|CNY|3.202
Denmark|krone|1|DKK|3.342
EMU|euro|1|EUR|24.935
Hongkong|dollar|1|HKD|2.960
Hungary|forint|100|HUF|6.431
Iceland|krona|100|ISK|16.590
IMF|SDR|1|XDR|30.498
India|rupee|100|INR|27.698
Indonesia|rupiah|1000|IDR|1.442
Israel|new shekel|1|ILS|6.219
Japan|yen|100|JPY|14.852
Malaysia|ringgit|1|MYR|4.881
Mexico|peso|1|MXN|1.380
New Zealand|dollar|1|NZD|13.915
Norway|krone|1|NOK|2.136
Philippines|peso|100|PHP|40.255
Poland|zloty|1|PLN|5.801
Romania|leu|1|RON|5.011
Singapore|dollar|1|SGD|17.088
South Africa|rand|1|ZAR|1.255
South Korea|won|100|KRW|1.693
Sweden|krona|1|SEK|2.134
Switzerland|franc|1|CHF|25.503
Thailand|baht|100|THB|62.950
Turkey|lira|100|TRY|71.779
United Kingdom|pound|1|GBP|28.979
USA|dollar|1|USD|23.131
     */
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Split_TextResponse_Into_Lines_Should_Return_CorrectNumberOfLines()
        {
            var responseText = @"10 May 2024 #90
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|15.285
Brazil|real|1|BRL|4.503
Bulgaria|lev|1|BGN|12.750
Canada|dollar|1|CAD|16.910
China|renminbi|1|CNY|3.202
Denmark|krone|1|DKK|3.342
EMU|euro|1|EUR|24.935";
            var lines = ResponseTextParser.SplitTextResponseIntoLines(responseText);
            lines.Should().HaveCount(9);
        }

        [Test]
        public void Parse_TextResponseLine_Should_Return_CorrectData()
        {
            var responseLine = @"10 May 2024 #90
Philippines|peso|100|PHP|40.255";
            var providerExchangeRate = ResponseTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeEquivalentTo(new ProviderExchangeRate("PHP", 40.255m));
        }

        [Test]
        public void Parse_TextResponseDateLine_Should_Return_Null()
        {
            var responseLine = @"10 May 2024 #90";
            var providerExchangeRate = ResponseTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }

        [Test]
        public void Parse_TextResponseHeaderLine_Should_Return_Null()
        {
            var responseLine = @"Country|Currency|Amount|Code|Rate";
            var providerExchangeRate = ResponseTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }

        [Test]
        public void Parse_TextResponseEmptyLine_Should_Return_Null()
        {
            var responseLine = @"";
            var providerExchangeRate = ResponseTextParser.ParseLine(responseLine, new System.Globalization.CultureInfo("en-US"), 3, 4);
            providerExchangeRate.Should().BeNull(null);
        }
    }
}