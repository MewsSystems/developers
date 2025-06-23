using ExchangeRateUpdate.Testing;
using ExchangeRateUpdater.Core.Adapters.CzechNationalBankApi;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.TestingSupport;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.IntegrationTests.Adapters.CzechNationalBankApi;

internal partial class CzechNationalBankApiAdapterTests
{
	public class TestContext : TestContext<CzechNationalBankApiAdapter>
	{
		protected override CzechNationalBankApiAdapter BuildSut(AutoMocker autoMocker) =>
			(CzechNationalBankApiAdapter)CoreInstanceBuilder.GetCzechNationalBankApiAdapter();

		public void AssertApiResponse(IEnumerable<ExchangeRate> exchangeRates)
		{
			var exchangeRatesArray = exchangeRates as ExchangeRate[] ?? exchangeRates.ToArray();
			Assert.That(exchangeRatesArray, Is.Not.Empty);
			Assert.That(exchangeRatesArray.All(y => y.TargetCurrency.Code == "CZK"), Is.True);
			Assert.That(exchangeRatesArray.Select(y => y.SourceCurrency.Code).Count(),
				Is.EqualTo(exchangeRatesArray.Select(x => x.SourceCurrency.Code).Distinct().Count()));
		}
	}
}