using ExchangeRateUpdate.Testing;
using ExchangeRateUpdater.Core.Models;
using ExchangeRateUpdater.Core.Providers;
using ExchangeRateUpdater.Core.TestingSupport;
using Moq.AutoMock;
using NUnit.Framework;

namespace ExchangeRateUpdater.Core.IntegrationTests.Providers;

internal partial class ExchangeRateProviderTests
{
	public class TestContext : TestContext<ExchangeRateProvider>
	{
		public readonly IEnumerable<Currency> Currencies = new[]
		{
			new Currency("AUD"),
			new Currency("BRL"),
			new Currency("CZK")
		};

		protected override ExchangeRateProvider BuildSut(AutoMocker autoMocker) =>
			(ExchangeRateProvider)CoreInstanceBuilder.GetExchangeRateProvider(CoreInstanceBuilder
				.GetCzechNationalBankApiAdapter);

		public void AssertExchangeRate(IEnumerable<ExchangeRate> result)
		{
			var exchangeRatesArray = result as ExchangeRate[] ?? result.ToArray();
			Assert.That(exchangeRatesArray, Is.Not.Empty);

			foreach (var exchangeRate in exchangeRatesArray)
			{
				Assert.That(Currencies.Contains(exchangeRate.SourceCurrency, new CurrencyComparer()));
			}
		}
	}
}