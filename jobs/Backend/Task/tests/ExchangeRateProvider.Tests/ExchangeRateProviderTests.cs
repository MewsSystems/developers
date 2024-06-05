using ExchangeRateProvider.Models;
using Moq;
using Shouldly;

namespace ExchangeRateProvider.Tests
{
	public class ExchangeRateProviderTests
	{
		[Fact]
		public async Task ProviderShouldIgnoreCurrenciesNotReturnedByTheApi()
		{
			// Arrange
			var bankApiClient = new Mock<IBankApiClient>();

			bankApiClient.Setup(client =>
				client.GetDailyExchangeRatesAsync(default))
					.Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
						[
							new BankCurrencyRate()
							{
								CurrencyCode = "EUR",
								Amount = 1,
								Rate = 24.2m
							}
						]
					));

			var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
			var provider = new ExchangeRateProvider(bankApiClient.Object);

			// Act
			var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

			// Assert
			exchangeRates.ShouldNotBeNull();
			exchangeRates.ShouldBeUnique();
			exchangeRates.ShouldContain(
				r =>
					r.SourceCurrency.Code == "EUR" && 
				    r.TargetCurrency.Code == "CZK" && 
				    r.Value == 24.2m);
		}

		[Fact]
		public async Task ProviderShouldTakeIntoAccountAmount()
		{
			// Arrange
			var bankApiClient = new Mock<IBankApiClient>();

			bankApiClient.Setup(client =>
					client.GetDailyExchangeRatesAsync(default))
				.Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
					[
						new BankCurrencyRate()
						{
							CurrencyCode = "JPY",
							Amount = 100,
							Rate = 14.528m
						}
					]
				));

			var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
			var provider = new ExchangeRateProvider(bankApiClient.Object);

			// Act
			var exchangeRates = await provider.GetExchangeRatesAsync(currencies);

			// Assert
			exchangeRates.ShouldNotBeNull();
			exchangeRates.ShouldBeUnique();
			exchangeRates.ShouldContain(
				r =>
					r.SourceCurrency.Code == "JPY" &&
					r.TargetCurrency.Code == "CZK" &&
					r.Value == 0.14528m);
		}
	}
}