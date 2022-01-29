using System;
using System.Collections.Generic;
using System.IO;
using ExchangeRateUpdater.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Dtos;
using ExchangeRateUpdater.ExternalServices.CzechNationalBank.HttpClient.Parsers;
using FluentAssertions;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests.ExternalServices.HttpClient.Parsers
{
    [TestFixture]
    public class ExchangeRateResponseParserTest : TestBase
    {
        [TestCaseSource(nameof(ProvideParseTestCases))]
        public void Parse_ShouldParseExchangeRates_WhenCorrectStringIsSupplied(string inputStr, IEnumerable<ExchangeRateDto> expectedExchangeRates)
        {
            // Given
            var streamToParse = CreateStreamFromString(inputStr);
            
            // When
            var resultExchangeRates = ExchangeRateResponseParser.Parse(streamToParse);

            // Then
            CompareTwoCollectionsDeeply<ExchangeRateDto>(
                expectedExchangeRates,
                resultExchangeRates, 
                new ExchangeRateDtoComparer()).Should().BeTrue();
        }

        static IEnumerable<TestCaseData> ProvideParseTestCases()
        {
            #region Test cases for parser
            yield return new TestCaseData(
                "27 Jan 2022 #19\n" +
                "Country|Currency|Amount|Code|Rate\n" +
                "Australia|dollar|1|AUD|15.492\n" +
                "Brazil|real|1|BRL|4.061",
                new List<ExchangeRateDto>
                {
                    new ExchangeRateDto
                    {
                        Country = "Australia",
                        CurrencyName = "dollar",
                        Amount = 1,
                        Currency = new Currency("AUD"),
                        Rate = (decimal)15.492
                    },
                    new ExchangeRateDto
                    {
                        Country = "Brazil",
                        CurrencyName = "real",
                        Amount = 1,
                        Currency = new Currency("BRL"),
                        Rate = (decimal)4.061
                    }
                }).SetName("Parse_ShouldParseExchangeRates_WhenCorrectStringIsSupplied_MultipleRates");
            
            yield return new TestCaseData(
                "27 Jan 2022 #19\n" +
                "Country|Currency|Amount|Code|Rate",
                new List<ExchangeRateDto> {}
            ).SetName("Parse_ShouldParseExchangeRates_WhenCorrectStringIsSupplied_NoRates");
            #endregion
        }

        class ExchangeRateDtoComparer : IComparer<ExchangeRateDto>
        {
            public int Compare(ExchangeRateDto x, ExchangeRateDto y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                var countryComparison = string.Compare(x.Country, y.Country, StringComparison.Ordinal);
                if (countryComparison != 0) return countryComparison;
                var currencyNameComparison = string.Compare(x.CurrencyName, y.CurrencyName, StringComparison.Ordinal);
                if (currencyNameComparison != 0) return currencyNameComparison;
                var amountComparison = Nullable.Compare(x.Amount, y.Amount);
                if (amountComparison != 0) return amountComparison;
                return Nullable.Compare(x.Rate, y.Rate);
            }
        }
    }
}