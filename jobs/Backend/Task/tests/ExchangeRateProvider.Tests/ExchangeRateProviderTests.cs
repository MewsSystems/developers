using ExchangeRateProvider.Http;
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
			var exchangeRateClient = new Mock<IExchangeRateClient>();

			exchangeRateClient.Setup(client =>
				client.GetDailyExchangeRates(default))
					.Returns(() =>
					{
						return Task.FromResult(
							(IEnumerable<CurrencyRate>)new[]
							{
								new CurrencyRate()
								{
									CurrencyCode = "EUR",
									Amount = 1,
									Rate = 24.2m
								}
							}
						);
					});

			var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
			var provider = new ExchangeRateProvider(exchangeRateClient.Object);

			// Act
			var exchangeRates = await provider.GetExchangeRates(currencies);

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
			var exchangeRateClient = new Mock<IExchangeRateClient>();

			exchangeRateClient.Setup(client =>
					client.GetDailyExchangeRates(default))
				.Returns(() =>
				{
					return Task.FromResult(
						(IEnumerable<CurrencyRate>)new[]
						{
							new CurrencyRate()
							{
								CurrencyCode = "JPY",
								Amount = 100,
								Rate = 14.528m
							}
						}
					);
				});
			var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
			var provider = new ExchangeRateProvider(exchangeRateClient.Object);

			// Act
			var exchangeRates = await provider.GetExchangeRates(currencies);

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