using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdate.Testing;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Core.Models;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.UnitTests.Adapters.CzechNationalBankApi;

internal partial class
	CzechNationalBankApiAdapterTests : TestFixture<CzechNationalBankApiAdapterTests.TestContext,
		CzechNationalBankApiAdapter>
{
	
	//I could generate a test case with all success httpStatus code
	[TestCase(200, true)]
	[TestCase(200, false)]
	[TestCase(210, true)]
	[TestCase(210, false)]
	public async Task GetExchangesRateAsync_ReturnsResultParsed_WhenSuccessResponse(int code, bool emptyResponse)
	{
		var statusCode = (HttpStatusCode)code;

		Context.WithHttpResponse(statusCode, emptyResponse);
		var response = await Context.Sut.GetExchangesRateAsync(CancellationToken.None);
		var exchangeRates = response as ExchangeRate[] ?? response.ToArray();
		Context.AssertSuccessResponse(exchangeRates, emptyResponse);
	}

	[Test]
	public void GetExchangesRateAsync_Throws_WhenCancellationTokenExpires()
	{
		using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));
		Assert.ThrowsAsync<TaskCanceledException>(() =>
			Context.WithHttpResponse(HttpStatusCode.OK).Sut.GetExchangesRateAsync(cts.Token));

	}
}