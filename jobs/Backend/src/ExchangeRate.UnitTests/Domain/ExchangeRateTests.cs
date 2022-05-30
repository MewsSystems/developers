using ExchangeRate.Domain;
using Xunit;

namespace ExchangeRate.UnitTests.Domain
{
	public class ExchangeRateTests
	{
		[Theory]
		[InlineData("CZK", "EUR", 25.25)]
		[InlineData("EUR", "CZK", 1)]
		[InlineData("USD", "EUR", 0.92)]
		public void ExchangeRate_ToString(string sourceCurrency, string targetCurrency, decimal rateValue)
		{
			var exchangeRate = new ExchangeRate.Domain.ExchangeRate(new Currency(sourceCurrency), new Currency(targetCurrency), rateValue);

			Assert.Equal($"{sourceCurrency}/{targetCurrency}={rateValue}", exchangeRate.ToString());
		}
	}
}
