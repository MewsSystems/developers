using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Testing;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.IntegrationTests.Providers;

internal partial class
	ExchangeRateProviderTests : TestFixture<ExchangeRateProviderTests.TestContext, ExchangeRateProvider>
{
	[Test]
	public async Task CanGetFilteredExchangeRates()
	{
		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
		var result = await Context.Sut.GetExchangeRatesAsync(Context.Currencies, cts.Token);
		Context.AssertExchangeRate(result);
	}

	[Test]
	public void WhenCurrenciesIsEmptyThrows()
	{
		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
		Assert.ThrowsAsync<ArgumentException>(() => Context.Sut.GetExchangeRatesAsync(Enumerable.Empty<Currency>(), cts.Token));
	}
}