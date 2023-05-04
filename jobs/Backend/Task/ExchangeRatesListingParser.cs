using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
    ///<inheritDoc/>
    internal class ExchangeRatesListingParser : IExchangeRatesListingParser
    {
        const string _ListingDateLinePattern = @"^\d+ [A-Za-z]+ \d+ #\d+$";
        const string _ExpectedExchangeRatesTableHeader = "Country|Currency|Amount|Code|Rate";
        const char _TableColumnSeparator = '|';

        const int _ExpectedExchangeRatesTableColumnsCount = 5;
        const int _AmountColumnIndex = 2;
        const int _CurrencyCodeColumnIndex = 3;
        const int _ExchangeRateColumnIndex = 4;

        const string _BankCurrencyCode = "CZK";

        ///<inheritDoc/>
        public ExchangeRatesListing ParseExchangeRatesListingString(string exchangeRatesListingString) {
            var listingLines = exchangeRatesListingString.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            if (listingLines.Length < 2)
                throw new ArgumentException("Invalid exchange rate listing string. The listing is expected to contain at least two lines (listing date and exchange rates table header).");
            var listingDate = ParseListingDate(listingLines[0]);

            // TODO allow any order of the table header columns and get corresponding indices
            ValidateExchangeRatesTableHeader(listingLines[1]);

            var exchangeRateLines = listingLines.Skip(2);
            var exchangeRates = exchangeRateLines.Select(line => ParseExchangeRateLine(line)).ToList();

            return new ExchangeRatesListing(listingDate, exchangeRates);
        }

        /// <summary>
        /// Parses one exchange rates table row. 
        /// </summary>
        /// <param name="exchangeRateLine">Exchange rate row string.</param>
        /// <returns>Parsed exchange rate instance.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the row does not have the expected format.
        /// </exception>
        private ExchangeRate ParseExchangeRateLine(string exchangeRateLine) {
            var exchangeRateInfoColumns = exchangeRateLine.Split(_TableColumnSeparator);
            if (exchangeRateInfoColumns.Length != _ExpectedExchangeRatesTableColumnsCount) {
                throw new ArgumentException($"Invalid exchange rate table row '{exchangeRateLine}' does not have expected number of columns, which is {_ExpectedExchangeRatesTableColumnsCount}.");
            }
            var codeColumnString = exchangeRateInfoColumns[_CurrencyCodeColumnIndex];
            var amountColumnString = exchangeRateInfoColumns[_AmountColumnIndex];
            if (!decimal.TryParse(amountColumnString, NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount)) {
                throw new ArgumentException($"Invalid amount value '{amountColumnString}' on line '{exchangeRateLine}'.");
            }
            var exchangeRateColumnString = exchangeRateInfoColumns[_ExchangeRateColumnIndex];
            if (!decimal.TryParse(exchangeRateColumnString, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var rate)) {
                throw new ArgumentException($"Invalid exchange rate value '{exchangeRateColumnString}' on line '{exchangeRateLine}'.");
            }

            var exchangeRate = rate / amount;

            return new ExchangeRate(new Currency(codeColumnString), new Currency(_BankCurrencyCode), exchangeRate);
        }

        /// <summary>
        /// Checks that the exchange rates table header matches expectations.
        /// </summary>
        /// <param name="exchangeRatesTableHeaderLine">String containing the exchnage rates table header line.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if the exchange rates table header does not look as expected.
        /// </exception>
        private void ValidateExchangeRatesTableHeader(string exchangeRatesTableHeaderLine) {
            if (exchangeRatesTableHeaderLine != _ExpectedExchangeRatesTableHeader) {
                throw new ArgumentException($"Invalid exchange rate listing string. Unexpected exchange rates table header '{exchangeRatesTableHeaderLine}'. Expected header was '{_ExpectedExchangeRatesTableHeader}'.");
            }
        }

        /// <summary>
        /// Parses the listing date from the listing date line.
        /// </summary>
        /// <param name="listingDateLine">String containing the listing date.</param>
        /// <returns>Listing date parsed from the line string.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the <paramref name="listingDateLine"/> has unexpected format or contains an invalid date.
        /// </exception>
        private DateOnly ParseListingDate(string listingDateLine) {
            if (!Regex.IsMatch(listingDateLine, _ListingDateLinePattern)) {
                throw new ArgumentException($"Invalid exchange rate listing string. Listing date line '{listingDateLine}' does not match expected pattern.");
            }
            var listingDateString = listingDateLine.Split('#', StringSplitOptions.TrimEntries)[0];
            if (!DateOnly.TryParse(listingDateString, out var listingDate)) {
                throw new ArgumentException($"Invalid exchange rate listing string. Invalid date '{listingDateString}' in the listing date line.");
            }

            return listingDate;
        }
    }
}
