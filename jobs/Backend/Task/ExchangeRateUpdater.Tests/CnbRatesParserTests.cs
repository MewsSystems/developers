using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.Tests
{
    /// <summary>
    /// Tests for CnbRatesParser - focused on parsing behavior only.
    /// </summary>
    public class CnbRatesParserTests
    {
        private static HashSet<string> AllCodes(params string[] codes) =>
            new(codes, StringComparer.OrdinalIgnoreCase);

        [Fact]
        public void Parse_ValidPayload_ParsesAndNormalizesRates()
        {
            // Arrange
            const string payload = @"14 Jan 2026 #9
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|23.456
Japan|yen|100|JPY|15.48";

            // Act
            var rates = CnbRatesParser.Parse(payload, AllCodes("USD", "JPY"), new Currency("CZK")).ToList();

            // Assert
            Assert.Equal(2, rates.Count);

            var usd = rates.Single(r => r.SourceCurrency.Code == "USD");
            Assert.Equal(23.456m, usd.Value); // amount=1, no normalization

            var jpy = rates.Single(r => r.SourceCurrency.Code == "JPY");
            Assert.Equal(0.1548m, jpy.Value); // 15.48 / 100 = 0.1548
        }

        [Fact]
        public void Parse_SkipsMalformedLinesWithoutFailing()
        {
            // Arrange: various malformed lines mixed with valid ones
            const string payload = @"14 Jan 2026 #9
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|23.456
Bad|Line|Missing|Columns
Japan|yen|INVALID|JPY|15.48
Eurozone|euro|1|EUR|";

            // Act
            var rates = CnbRatesParser.Parse(payload, AllCodes("USD", "JPY", "EUR"), new Currency("CZK")).ToList();

            // Assert: only USD parses (JPY has invalid amount, EUR has empty rate)
            Assert.Single(rates);
            Assert.Equal("USD", rates[0].SourceCurrency.Code);
        }

        [Fact]
        public void Parse_HandlesCommaAsDecimalSeparator()
        {
            // Arrange
            const string payload = @"14 Jan 2026 #9
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|23,456";

            // Act
            var rates = CnbRatesParser.Parse(payload, AllCodes("USD"), new Currency("CZK")).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal(23.456m, rates[0].Value);
        }

        [Fact]
        public void Parse_FiltersToRequestedCodes()
        {
            // Arrange
            const string payload = @"14 Jan 2026 #9
Country|Currency|Amount|Code|Rate
USA|dollar|1|USD|23.456
Eurozone|euro|1|EUR|25.123
Japan|yen|100|JPY|15.48";

            // Act: only request USD
            var rates = CnbRatesParser.Parse(payload, AllCodes("USD"), new Currency("CZK")).ToList();

            // Assert
            Assert.Single(rates);
            Assert.Equal("USD", rates[0].SourceCurrency.Code);
        }
    }
}
