using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Parsers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ExchangeRateUpdater.Tests.Parsers
{
    public class CzechNationalBankTextRateParserTests
    {
        private readonly CzechNationalBankTextRateParser _parser;

        public CzechNationalBankTextRateParserTests()
        {
            var mockLogger = new Mock<ILogger<CzechNationalBankTextRateParser>>();
            _parser = new CzechNationalBankTextRateParser(5, mockLogger.Object);
        }

        [Fact]
        public void Parse_ParsesValidLine_Correctly()
        {
            var raw = @"
11 Jun 2025 #123
Country|Currency|Amount|Code|Rate
United States|Dollar|1|USD|22,123
";

            var filter = new List<Currency> { new Currency("USD") };
            var result = _parser.Parse(raw, filter, new Currency("CZK")).ToList();

            Assert.Single(result);
            var rate = result[0];
            Assert.Equal("CZK", rate.SourceCurrency.Code);
            Assert.Equal("USD", rate.TargetCurrency.Code);
            Assert.Equal(22.123m, rate.Value);
        }

        [Fact]
        public void Parse_SkipsLines_WithTooFewColumns()
        {
            var raw = @"
11 Jun 2025 #123
Country|Currency|Amount|Code|Rate
Broken|Line|Only|3
United States|Dollar|1|USD|22,123
";

            var filter = new List<Currency> { new Currency("USD") };
            var result = _parser.Parse(raw, filter, new Currency("CZK")).ToList();

            Assert.Single(result);
            Assert.Equal("USD", result[0].TargetCurrency.Code);
        }

        [Fact]
        public void Parse_SkipsCurrencies_NotInFilter()
        {
            var raw = @"
11 Jun 2025 #123
Country|Currency|Amount|Code|Rate
United States|Dollar|1|USD|22,123
Eurozone|Euro|1|EUR|25,456
";

            var filter = new List<Currency> { new Currency("EUR") };
            var result = _parser.Parse(raw, filter, new Currency("CZK")).ToList();

            Assert.Single(result);
            Assert.Equal("EUR", result[0].TargetCurrency.Code);
        }

        [Fact]
        public void Parse_NormalizesRate_BasedOnAmount()
        {
            var raw = @"
11 Jun 2025 #123
Country|Currency|Amount|Code|Rate
Japan|Yen|100|JPY|50,000
";

            var filter = new List<Currency> { new Currency("JPY") };
            var result = _parser.Parse(raw, filter, new Currency("CZK")).ToList();

            Assert.Single(result);
            Assert.Equal("JPY", result[0].TargetCurrency.Code);
            Assert.Equal(0.500, (double)result[0].Value); // 50,000 / 100
        }

        [Fact]
        public void Parse_ReturnsEmpty_WhenNoLinesMatch()
        {
            var raw = @"
11 Jun 2025 #123
Country|Currency|Amount|Code|Rate
United States|Dollar|1|USD|22,123
";

            var filter = new List<Currency> { new Currency("EUR") }; // no EUR present
            var result = _parser.Parse(raw, filter, new Currency("CZK")).ToList();

            Assert.Empty(result);
        }
    }
}