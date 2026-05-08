using FluentAssertions;
using ExchangeRateUpdater.Parsers;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Tests.TestData;
using Microsoft.Extensions.Logging.Abstractions;

namespace ExchangeRateUpdater.Tests.Parsers
{
    public class CnbDataParserTests
    {
        private readonly CnbDataParser _parser;

        public CnbDataParserTests()
        {
            _parser = new CnbDataParser(NullLogger<CnbDataParser>.Instance);
        }

        [Fact]
        public void Parse_ValidData_ReturnsCorrectExchangeRates()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-full-response.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert - Verify all 9 currencies with all fields
            result.Should().HaveCount(9);
            result.Should().Contain(r => r.Country == "Australia" && r.CurrencyName == "dollar" && r.Amount == 1 && r.Code == "AUD" && r.Rate == 13.986m);
            result.Should().Contain(r => r.Country == "Brazil" && r.CurrencyName == "real" && r.Amount == 1 && r.Code == "BRL" && r.Rate == 3.858m);
            result.Should().Contain(r => r.Country == "Canada" && r.CurrencyName == "dollar" && r.Amount == 1 && r.Code == "CAD" && r.Rate == 15.053m);
            result.Should().Contain(r => r.Country == "Switzerland" && r.CurrencyName == "franc" && r.Amount == 1 && r.Code == "CHF" && r.Rate == 25.035m);
            result.Should().Contain(r => r.Country == "China" && r.CurrencyName == "renminbi" && r.Amount == 1 && r.Code == "CNY" && r.Rate == 2.863m);
            result.Should().Contain(r => r.Country == "EMU" && r.CurrencyName == "euro" && r.Amount == 1 && r.Code == "EUR" && r.Rate == 24.280m);
            result.Should().Contain(r => r.Country == "United Kingdom" && r.CurrencyName == "pound" && r.Amount == 1 && r.Code == "GBP" && r.Rate == 28.022m);
            result.Should().Contain(r => r.Country == "Japan" && r.CurrencyName == "yen" && r.Amount == 100 && r.Code == "JPY" && r.Rate == 13.278m);
            result.Should().Contain(r => r.Country == "USA" && r.CurrencyName == "dollar" && r.Amount == 1 && r.Code == "USD" && r.Rate == 20.774m);
        }

        [Fact]
        public void Parse_CurrencyCodesAreCaseInsensitive_ConvertsToUpperCase()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-lowercase-code.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Code.Should().Be("USD");
        }

        [Fact]
        public void Parse_EmptyData_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-empty.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("Cannot parse empty or null data.");
        }

        [Fact]
        public void Parse_NullData_ThrowsException()
        {
            // Parse test data & Assert
            var act = () => _parser.Parse(null!);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("Cannot parse empty or null data.");
        }

        [Fact]
        public void Parse_LessThanThreeLines_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-less-than-three-lines.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Expected at least 3 lines*");
        }

        [Fact]
        public void Parse_InvalidColumnHeaders_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-headers.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Invalid column headers*");
        }

        [Fact]
        public void Parse_InvalidNumberOfColumns_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-missing-column.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Invalid number of columns*")
                .Which.LineNumber.Should().Be(3);
        }

        [Fact]
        public void Parse_InvalidAmount_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-bad-amount.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Failed to parse Amount 'invalid' as integer*");
        }

        [Fact]
        public void Parse_NegativeAmount_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-negative-amount.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Amount must be positive*");
        }

        [Fact]
        public void Parse_InvalidRate_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-bad-rate.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Failed to parse Rate 'notanumber' as decimal*");
        }

        [Fact]
        public void Parse_NegativeRate_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-negative-rate.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Rate must be positive*");
        }

        [Fact]
        public void Parse_InvalidCurrencyCode_ThrowsException()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("invalid-short-code.txt");

            // Parse test data & Assert
            var act = () => _parser.Parse(rawData);
            act.Should().Throw<CnbDataParsingException>()
                .WithMessage("*Invalid currency code 'XY'*");
        }

        [Fact]
        public void Parse_DecimalWithDifferentCulture_ParsesCorrectly()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-single-currency.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Country.Should().Be("USA");
            result[0].CurrencyName.Should().Be("dollar");
            result[0].Amount.Should().Be(1);
            result[0].Code.Should().Be("USD");
            result[0].Rate.Should().Be(20.774m);
        }

        [Fact]
        public void Parse_AmountOf100_ParsesCorrectly()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-amount-100.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Country.Should().Be("Japan");
            result[0].CurrencyName.Should().Be("yen");
            result[0].Amount.Should().Be(100);
            result[0].Code.Should().Be("JPY");
            result[0].Rate.Should().Be(13.278m);
        }

        [Fact]
        public void Parse_AmountOf1000_ParsesCorrectly()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-amount-1000.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Country.Should().Be("Indonesia");
            result[0].CurrencyName.Should().Be("rupiah");
            result[0].Amount.Should().Be(1000);
            result[0].Code.Should().Be("IDR");
            result[0].Rate.Should().Be(1.238m);
        }

        [Fact]
        public void Parse_WhitespaceInFields_TrimsCorrectly()
        {
            // Read test file
            var rawData = TestDataLoader.LoadTestFile("valid-whitespace-fields.txt");

            // Parse test data
            var result = _parser.Parse(rawData).ToList();

            // Assert
            result.Should().HaveCount(1);
            result[0].Country.Should().Be("USA");
            result[0].CurrencyName.Should().Be("dollar");
            result[0].Amount.Should().Be(1);
            result[0].Code.Should().Be("USD");
            result[0].Rate.Should().Be(20.774m);
        }
    }
}
