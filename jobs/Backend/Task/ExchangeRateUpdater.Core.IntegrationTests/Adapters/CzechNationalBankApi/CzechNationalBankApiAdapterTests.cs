using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Testing;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.IntegrationTests.Adapters.CzechNationalBankApi;

internal partial class
	CzechNationalBankApiAdapterTests : TestFixture<CzechNationalBankApiAdapterTests.TestContext,
		CzechNationalBankApiAdapter>
{
	[Test]
	public async Task CanGetExchangeRatesFromApi()
	{
		using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
		var result = await Context.Sut.GetExchangesRateAsync(cts.Token);
		Context.AssertApiResponse(result);
	}
}