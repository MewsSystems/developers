using ExchangeRateUpdater.Application.Interfaces;
using ExchangeRateUpdater.Application.Services;
using ExchangeRateUpdater.Domain.Entities;
using FluentAssertions;
using Moq;

namespace ExchangeRateUpdater.Application.Tests.Services;

public class ExchangeRateProviderTests
{
	[Fact]
	public async Task GetExchangeRates_ShouldReturnEmptyListIfCurrenciesIsEmpty()
	{
		var date = new DateOnly(2024, 1, 1);
		
		var client = new Mock<ICzechNationalBankExchangeRateClient>();
		List<ExchangeRate> rates =
		[
			new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 1.2m),
		];
		client.Setup(m => m.FetchExchangeRates(It.IsAny<string>(), date).Result).Returns(rates);
		var provider = new ExchangeRateProvider(client.Object);

		var exchangeRates = await provider.GetExchangeRates(date, []);

		exchangeRates.Should().BeEmpty();
	}

	[Fact]
	public async Task GetExchangeRates_ShouldOnlyReturnRatesOfRequestedCurrencies()
	{
		var date = new DateOnly(2024, 1, 1);
		var client = new Mock<ICzechNationalBankExchangeRateClient>();
		List<ExchangeRate> rates =
		[
			new ExchangeRate(new Currency("GBP"), new Currency("CZK"), 1.2m),
			new ExchangeRate(new Currency("AUD"), new Currency("CZK"), 7.41m),
			new ExchangeRate(new Currency("USD"), new Currency("CZK"), 0.0231m),
		];
		client.Setup(m => m.FetchExchangeRates(It.IsAny<string>(), date).Result).Returns(rates);
		List<Currency> desiredCurrencies = [rates.ElementAt(0).SourceCurrency, rates.ElementAt(1).SourceCurrency];
		var provider = new ExchangeRateProvider(client.Object);

		var exchangeRates = (await provider.GetExchangeRates(date, desiredCurrencies)).ToList();

		desiredCurrencies.Should().HaveSameCount(desiredCurrencies).And.OnlyHaveUniqueItems();
		exchangeRates.Select(r => r.SourceCurrency).Should().Equal(desiredCurrencies);
	}
}
