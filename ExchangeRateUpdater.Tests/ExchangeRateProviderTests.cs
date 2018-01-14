using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExchangeRateUpdater.ExchangeRateProviders;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Tests
{
	[TestClass]
	public class ExchangeRateProviderTests
	{
		[TestMethod]
		public void IfProvidersAreEmptyThenReturnNoRates()
		{
			var provider = new ExchangeRateProvider(new IExchangeRateProvider[0]);
			var result = provider.GetExchangeRates(new[] { new Currency("EUR") }).ToArray();

			Assert.AreEqual(0, result.Length);
		}

		[TestMethod]
		public void IfExistsMoreProvidersThenReturnRatesFromAll()
		{
			var subprovider1 = new FakeProvider(new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27));
			var subprovider2 = new FakeProvider(new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22));

			var provider = new ExchangeRateProvider(new[] { subprovider1, subprovider2 });
			var result = provider.GetExchangeRates(new[] { new Currency("EUR"), new Currency("USD") }).ToArray();

			Assert.AreEqual(2, result.Length);
			Assert.AreEqual(27, result.Single(r => r.SourceCurrency.Code == "EUR").Value);
			Assert.AreEqual(22, result.Single(r => r.SourceCurrency.Code == "USD").Value);
		}

		[TestMethod]
		public void IfCurrencyIsNotRequestedThenDontReturnIt()
		{
			var subprovider = new FakeProvider(
				new ExchangeRate(new Currency("EUR"), new Currency("CZK"), 27),
				new ExchangeRate(new Currency("USD"), new Currency("CZK"), 22));

			var provider = new ExchangeRateProvider(new[] { subprovider});
			var result = provider.GetExchangeRates(new[] { new Currency("EUR") }).ToArray();

			Assert.AreEqual(1, result.Length);
			Assert.AreEqual("EUR", result.Single().SourceCurrency.Code);
		}

		private class FakeProvider : IExchangeRateProvider
		{
			private readonly ExchangeRate[] rates;

			public FakeProvider(params ExchangeRate[] rates)
			{
				this.rates = rates;
			}

			public IEnumerable<ExchangeRate> GetExchangeRates(DateTime date)
			{
				return rates;
			}
		}
	}
}
