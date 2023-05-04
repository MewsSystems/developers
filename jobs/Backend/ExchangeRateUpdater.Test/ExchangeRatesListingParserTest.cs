using NUnit.Framework;

namespace ExchangeRateUpdater.Test
{
    public class ExchangeRatesListingParserTest
    {
        const string _ExpectedExchangeRatesTableHeader = "Country|Currency|Amount|Code|Rate";

        [Test]
        public void TestParseExchangeRatesListingFromEmptyString() {
            var testObj = new ExchangeRatesListingParser();

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(string.Empty); });
            Assert.AreEqual("Invalid exchange rate listing string. The listing is expected to contain at least two lines (listing date and exchange rates table header).",
                ex.Message);
        }

        [Test]
        public void TestParseExchangeRatesListingFromOneLineString() {
            var testObj = new ExchangeRatesListingParser();
            var oneLineString = "02 May 2023 #84\n";

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(oneLineString); });
            Assert.AreEqual("Invalid exchange rate listing string. The listing is expected to contain at least two lines (listing date and exchange rates table header).",
                ex.Message);
        }

        [Test]
        public void TestParseExchangeRatesListingWithInvalidListingDateLineFormat() {
            var testObj = new ExchangeRatesListingParser();
            var invalidListingDateLine = "02 May 2023";
            var tableHeaderLine = "Country|Currency|Amount|Code|Rate";
            var listingString = invalidListingDateLine + "\n" + tableHeaderLine;

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid exchange rate listing string. Listing date line '{invalidListingDateLine}' does not match expected pattern.",
                ex.Message);
        }

        [TestCase("30 February 2020")]
        [TestCase("15 Juny 2020")]
        [TestCase("50 Jan 2020")]
        public void TestParseExchangeRatesListingWithInvalidListingDate(string invalidDateString) {
            var testObj = new ExchangeRatesListingParser();
            var invalidListingDateLine = $"{invalidDateString} #84";
            var tableHeaderLine = "Country|Currency|Amount|Code|Rate";
            var listingString = invalidListingDateLine + "\n" + tableHeaderLine;

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid exchange rate listing string. Invalid date '{invalidDateString}' in the listing date line.",
                ex.Message);
        }

        [TestCase("Country|Currency|Code|Rate")]
        [TestCase("Country|Currency|Amount|Code|Rate|Bonus")]
        [TestCase("Country|Currency|Rate|Code|Amount")]
        public void TestParseExchangeRatesListingWithUnexpectedExchangeRatesTableHeader(string unexpectedExchangeRatesTableHeaderLine) {
            var testObj = new ExchangeRatesListingParser();
            var listingDateLine = "02 May 2023 #84";
            var listingString = string.Join("\n", listingDateLine, unexpectedExchangeRatesTableHeaderLine);

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid exchange rate listing string. Unexpected exchange rates table header '{unexpectedExchangeRatesTableHeaderLine}'. Expected header was '{_ExpectedExchangeRatesTableHeader}'.",
                ex.Message);
        }

        [TestCase("Australia|1|AUD|14.410")]
        [TestCase("Australia|dollar|1||AUD|14.410")]
        public void TestParseExchangeRatesListingWithUnexpectedExchangeRateRowFormat(string unexpectedExchangeRowString) {
            var testObj = new ExchangeRatesListingParser();
            var listingDateLine = "01 May 2023 #84";
            var listingString = string.Join("\n", listingDateLine, _ExpectedExchangeRatesTableHeader, unexpectedExchangeRowString);

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid exchange rate table row '{unexpectedExchangeRowString}' does not have expected number of columns, which is 5.",
                ex.Message);
        }

        [TestCase("1.1")]
        [TestCase("one")]
        public void TestParseExchangeRatesListingWithInvalidAmountValueInExchangeRateRow(string invalidAmount) {
            var testObj = new ExchangeRatesListingParser();
            var invalidExchangeRateRow = $"Canada|dollar|{invalidAmount}|CAD|15.849";
            var listingDateLine = "01 May 2023 #84";
            var listingString = string.Join("\n", listingDateLine, _ExpectedExchangeRatesTableHeader, invalidExchangeRateRow);

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid amount value '{invalidAmount}' on line '{invalidExchangeRateRow}'.",
                ex.Message);
        }

        [TestCase("1,7")]
        [TestCase("milion")]
        public void TestParseExchangeRatesListingWithInvalidRateValueInExchangeRateRow(string invalidRate) {
            var testObj = new ExchangeRatesListingParser();
            var invalidExchangeRateRow = $"Canada|dollar|1|CAD|{invalidRate}";
            var listingDateLine = "01 May 2023 #84";
            var listingString = string.Join("\n", listingDateLine, _ExpectedExchangeRatesTableHeader, invalidExchangeRateRow);

            var ex = Assert.Throws<ArgumentException>(delegate { testObj.ParseExchangeRatesListingString(listingString); });
            Assert.AreEqual($"Invalid exchange rate value '{invalidRate}' on line '{invalidExchangeRateRow}'.",
                ex.Message);
        }

        [Test]
        public void TestParseExchangeRatesListingWithValidExchangeRateListingString() {
            var testObj = new ExchangeRatesListingParser();
            var listingDateLine = "01 May 2023 #84";
            var firstExchangeRateLine = "Bangladesh|taka|100|BDT|20.180";
            var secondExchangeRateLine = "Barbados|dollar|1|BBD|10.716";
            var listingString = string.Join("\n", listingDateLine, _ExpectedExchangeRatesTableHeader, firstExchangeRateLine, secondExchangeRateLine);
            
            var parsedListing = testObj.ParseExchangeRatesListingString(listingString);

            Assert.AreEqual(new DateOnly(2023, 5, 1), parsedListing.ListingDate);
            Assert.AreEqual(2, parsedListing.ExchangeRates.Count);
            AssertExpectedExchangeRate(parsedListing.ExchangeRates[0], new Currency("BDT"), new Currency("CZK"), 0.2018m);
            AssertExpectedExchangeRate(parsedListing.ExchangeRates[1], new Currency("BBD"), new Currency("CZK"), 10.716m);
        }

        private void AssertExpectedExchangeRate(ExchangeRate exchangeRateToCheck, Currency expectedSourceCurrency, Currency expectedTargetCurrency, decimal expectedValue) {
            Assert.AreEqual(expectedSourceCurrency, exchangeRateToCheck.SourceCurrency);
            Assert.AreEqual(expectedTargetCurrency, exchangeRateToCheck.TargetCurrency);
            Assert.AreEqual(expectedValue, exchangeRateToCheck.Value);
        }
    }
}