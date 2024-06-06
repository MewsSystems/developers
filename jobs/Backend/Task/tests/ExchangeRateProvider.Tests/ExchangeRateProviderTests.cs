using ExchangeRateProvider.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;

namespace ExchangeRateProvider.Tests;

public class ExchangeRateProviderTests
{
	[Fact]
	public async Task ProviderShouldIgnoreCurrenciesNotReturnedByTheApi()
	{
		// Arrange
		var cache = new MemoryCache(new MemoryCacheOptions());
		var bankApiClient = new Mock<IBankApiClient>();
		var logger = new Mock<ILogger>();

		bankApiClient.Setup(client =>
			client.GetDailyExchangeRatesAsync(default))
				.Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
					[
						new BankCurrencyRate(1, "EUR", 24.2m)
					]
				));

		var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
		var provider = new ExchangeRateProvider(bankApiClient.Object, cache, logger.Object);

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
		var cache = new MemoryCache(new MemoryCacheOptions());
		var bankApiClient = new Mock<IBankApiClient>();
		var logger = new Mock<ILogger>();

		bankApiClient.Setup(client =>
				client.GetDailyExchangeRatesAsync(default))
			.Returns(() => Task.FromResult((IEnumerable<BankCurrencyRate>)
				[
					new BankCurrencyRate(100, "JPY", 14.528m)
				]
			));

		var currencies = new[] { "USD", "EUR", "JPY" }.Select(c => new Currency(c));
		var provider = new ExchangeRateProvider(bankApiClient.Object, cache, logger.Object);

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