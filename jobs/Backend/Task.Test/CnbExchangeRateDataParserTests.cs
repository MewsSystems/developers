namespace ExchangeReaderUpdater.Test
{
    using ExchangeRateUpdater.Models;
    using ExchangeRateUpdater.Parsers;
    using FluentAssertions;
    using System.Linq;
    using Xunit;

    public class CnbExchangeRateDataParserTests
    {
        private readonly CnbExchangeRateDataParser _parser = new CnbExchangeRateDataParser();

        [Fact]
        public void parse_valid_single_line_returns_exchange_rate()
        {
            var currencies = new[] { new Currency("USD") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|21.456\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(1);
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.SourceCurrency.Code.Should().Be("USD");
                    first.TargetCurrency.Code.Should().Be("CZK");
                    first.Value.Should().Be(21.456m);
                });
        }

        [Fact]
        public void parse_multiple_lines_returns_exchange_rates()
        {
            var currencies = new[] { new Currency("USD"), new Currency("EUR"), new Currency("AUD") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|21.456\nEMU|euro|1|EUR|24.285\nAustralia|dollar|1|AUD|14.004";
            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(3);
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.SourceCurrency.Code.Should().Be("USD");
                    first.TargetCurrency.Code.Should().Be("CZK");
                    first.Value.Should().Be(21.456m);
                },
                second =>
                {
                    second.SourceCurrency.Code.Should().Be("EUR");
                    second.TargetCurrency.Code.Should().Be("CZK");
                    second.Value.Should().Be(24.285m);
                },
                third =>
                {
                    third.SourceCurrency.Code.Should().Be("AUD");
                    third.TargetCurrency.Code.Should().Be("CZK");
                    third.Value.Should().Be(14.004m);
                });
        }

        [Fact]
        public void parse_multiple_lines_returns_filtered_exchange_rates()
        {
            var currencies = new[] { new Currency("EUR"), new Currency("AUD") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|21.456\nEMU|euro|1|EUR|24.285\nAustralia|dollar|1|AUD|14.004";
            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(2);
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.SourceCurrency.Code.Should().Be("EUR");
                    first.TargetCurrency.Code.Should().Be("CZK");
                    first.Value.Should().Be(24.285m);
                },
                second =>
                {
                    second.SourceCurrency.Code.Should().Be("AUD");
                    second.TargetCurrency.Code.Should().Be("CZK");
                    second.Value.Should().Be(14.004m);
                });
        }

        [Fact]
        public void parse_invalid_column_count_skips_line()
        {
            var currencies = new[] { new Currency("USD") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(0);
        }

        [Fact]
        public void parse_whitespace_trims_code()
        {
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\n  USA  |Dollar|1|  USD  |21.456\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(1);
            result.Should().SatisfyRespectively(
                first =>
                {
                    first.SourceCurrency.Code.Should().Be("USD");
                    first.TargetCurrency.Code.Should().Be("CZK");
                    first.Value.Should().Be(21.456m);
                });
        }

        [Fact]
        public void zero_rate_skips_line()
        {
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|0\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(0);
        }

        [Fact]
        public void non_numeric_rate_skips_line()
        {
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|not_a_number\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(0);
        }

        [Fact]
        public void parse_various_newline_formats()
        {
            var currencies = new[] { new Currency("USD"), new Currency("EUR") };
            var data = "25 Feb 2025 #11\r\nCountry|Currency|Amount|Code|Rate\r\nUSA|Dollar|1|USD|21.456\nEMU|euro|1|EUR|24.285\rAustralia|dollar|1|AUD|14.004";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().HaveCount(2);
            result.Select(r => r.SourceCurrency.Code).Should().Contain(new[] { "USD", "EUR" });
        }

        [Fact]
        public void amount_zero_skips_line()
        {
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|0|USD|21.456\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().BeEmpty();
        }

        [Fact]
        public void negative_rate_skips_line()
        {
            var currencies = new[] { new Currency("USD"), new Currency("CZK") };
            var data = "25 Feb 2025 #11\nCountry|Currency|Amount|Code|Rate\nUSA|Dollar|1|USD|-21.456\n";

            var result = _parser.Parse(data, currencies).ToList();

            result.Should().BeEmpty();
        }
    }
}
