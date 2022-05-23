using System.ComponentModel.DataAnnotations;
using ExchangeRate.Domain;
using Xunit;

namespace ExchangeRate.UnitTests.Domain
{
	public class CurrencyTests
	{
		[Theory]
		[InlineData("CZK")]
		[InlineData("SVK")]
		[InlineData("EUR")]
		[InlineData("USD")]
		public void Currency_ToString(string expected)
		{
			var currency = new Currency(expected);

			var result = currency.ToString();

			Assert.Equal(expected, result);
		}

		[Theory]
		[InlineData("T")]
		[InlineData("TE")]
		[InlineData("TEST")]
		[InlineData("tes")]
		[InlineData("TEST123")]
		[InlineData("test123")]
		public void Currency_ShouldThrowValidationException(string expected)
		{
			var exception = Assert.Throws<ValidationException>(() => new Currency(expected));
			Assert.Equal($"Currency code {expected} should have 3 characters. Check ISO 4217", exception.Message);
		}

		[Theory]
		[InlineData(null)]
		public void Currency_ShouldThrowArgumentException(string expected)
		{
			var exception = Assert.Throws<ArgumentNullException>(() => new Currency(expected));
			Assert.Equal("Currency code cannot be empty", exception.Message);
		}
	}
}
