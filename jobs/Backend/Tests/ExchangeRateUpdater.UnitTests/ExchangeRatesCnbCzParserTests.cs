using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Parse;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExchangeRateUpdater.UnitTests
{
    public class ExchangeRatesCnbCzParserTests
    {
        private Config config;

        public ExchangeRatesCnbCzParserTests()
        {
            config = new Config()
            {
                ExchangeRateFieldsDelimeter = "|",
                ExchangeRatesForCurrency = "CZK",
                ExchangeRatesUrl = ""
            };
        }

        public static IEnumerable<object[]> ExpectedRatesTxt = new List<object[]>
        {
            new object[4]
            {
                @"18 Mar 2022 #55
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.621
Brazil|real|1|BRL|4.454
Bulgaria|lev|1|BGN|12.702
Canada|dollar|1|CAD|17.859
China|renminbi|1|CNY|3.548",
                5,
                "CNY",
                3.548M
            },
            new object[4]
            {
                @"18 Mar 2022 #55
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.621
Brazil|real|1|BRL|4.454
Bulgaria|lev|1|BGN|12.702
Canada|dollar|1|CAD|17.859
China|renminbi|1|CNY|3.548
Croatia|kuna|1|HRK|3.283
Denmark|krone|1|DKK|3.338
EMU|euro|1|EUR|24.840
Hongkong|dollar|1|HKD|2.885
Hungary|forint|100|HUF|6.618
Iceland|krona|100|ISK|17.383
IMF|SDR|1|XDR|31.216
India|rupee|100|INR|29.651
Indonesia|rupiah|1000|IDR|1.574
Israel|new shekel|1|ILS|6.946
Japan|yen|100|JPY|18.903
Malaysia|ringgit|1|MYR|5.382
Mexico|peso|1|MXN|1.100
New Zealand|dollar|1|NZD|15.502
Norway|krone|1|NOK|2.562
Philippines|peso|100|PHP|43.110
Poland|zloty|1|PLN|5.270
Romania|leu|1|RON|5.020
Singapore|dollar|1|SGD|16.615
South Africa|rand|1|ZAR|1.503
South Korea|won|100|KRW|1.858
Sweden|krona|1|SEK|2.381
Switzerland|franc|1|CHF|24.078
Thailand|baht|100|THB|67.616
Turkey|lira|1|TRY|1.524
United Kingdom|pound|1|GBP|29.597
USA|dollar|1|USD|22.567",
                32,
                "USD",
                22.567M
            }
        };

        [Theory]
        [MemberData(nameof(ExpectedRatesTxt))]
        public void ParsingExpectedStringShouldHaveExpectedResults(string txt, int count, string lastRateCode, decimal lastRateValue)
        {
            var parser = new ExchangeRatesCnbCzParser(config, new NullLogger<ExchangeRatesCnbCzParser>());

            var parsedRates = parser.ParseRates(txt);

            Assert.Equal(count, parsedRates.Count());
            if (count > 0)
            {
                var lastRate = parsedRates.Last();
                Assert.Equal(lastRateCode, lastRate.SourceCurrency.Code);
                Assert.Equal("CZK", lastRate.TargetCurrency.Code);
                Assert.Equal(lastRateValue, lastRate.Value);
            }
        }

        [Fact]
        public void ParsingUnexpectedStringShouldThrow()
        {
            var parser = new ExchangeRatesCnbCzParser(config, new NullLogger<ExchangeRatesCnbCzParser>());
            Assert.ThrowsAny<Exception>(() => parser.ParseRates("foo|bar\nmalformed|data"));
        }

        [Fact]
        public void ParsingEmptyStringShouldThrow()
        {
            var parser = new ExchangeRatesCnbCzParser(config, new NullLogger<ExchangeRatesCnbCzParser>());
            Assert.ThrowsAny<Exception>(() => parser.ParseRates(""));
        }
    }
}