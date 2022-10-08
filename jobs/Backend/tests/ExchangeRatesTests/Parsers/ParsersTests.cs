using FluentAssertions;
using Xunit;

namespace ExchangeRates.Tests.Parsers
{
	[Trait("Category", "Parsers unit tests")]
	public class ParsersTests : IClassFixture<ParsersTestsFixture>
	{		
		private readonly ParsersTestsFixture fixture;		

		public ParsersTests(ParsersTestsFixture fixture)
		{
			this.fixture = fixture;
		}		

		[Fact]
		public void CnbParser_OKResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_ok.txt");
			var expectedOutput = fixture.GetExchangeRates("CZK", "EUR", "USD");

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEquivalentTo(expectedOutput);
		}

		[Fact]
		public void CnbParser_OnlyHeaders_EmptyResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_only_header.txt");			

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEmpty();
		}

		[Fact]
		public void CnbParser_UnexpectedLinesSeparator_EmptyResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_unexpected_line_separator.txt");

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEmpty();
		}

		[Fact]
		public void CnbParser_UnexpectedColumnsSeparator_EmptyResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_unexpected_column_separator.txt");

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEmpty();
		}

		[Fact]
		public void CnbParser_UnexpectedNumberOfColumns_EmptyResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_unexpected_number_of_columns.txt");

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEmpty();
		}

		[Fact]
		public void CnbParser_UnexpectedFormat_EmptyResultExpected()
		{
			// Arrange
			var parser = fixture.GetCnbParser();
			var input = fixture.GetCnbData("Files.cnb_fx_rates_unexpected_format.json");

			// Act
			var result = parser.ParserData(input);

			// Assert
			result.Should().BeEmpty();
		}
	}
}