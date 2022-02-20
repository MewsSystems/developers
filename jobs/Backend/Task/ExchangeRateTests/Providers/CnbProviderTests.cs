using System;
using System.Collections.Generic;
using System.Linq;
using Common.Model;
using Common.Providers;
using ExchangeRateUpdater.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NuGet.Frameworks;

namespace ExchangeRateUpdater.Providers.Tests
{
	[TestClass()]
	public class CnbProviderTests
	{
		[TestMethod()]
		public void GetExchangeRatesAsync_CzkCurrencyOnInput_CzkNotInResult()
		{
			CnbProvider provider = new CnbProvider();

			List<Currency> currencies = new List<Currency> {new Currency("CZK")};
			IEnumerable<ExchangeRate> rates = provider.GetExchangeRatesAsync(currencies).Result;

			Assert.IsNotNull(rates);
			Assert.IsTrue(!rates.Any());
		}

		[TestMethod()]
		public void GetExchangeRatesAsync_RedundantInputs_RedundancyIsIgnored()
		{
			CnbProvider provider = new CnbProvider();

			List<Currency> currencies = new List<Currency> { new Currency("USD") , new Currency("USD") };
			IEnumerable<ExchangeRate> rates = provider.GetExchangeRatesAsync(currencies).Result;

			Assert.IsNotNull(rates);
			Assert.IsTrue(rates.Count() == 1);
		}

		[TestMethod()]
		public void GetExchangeRatesAsync_PastDate_CorrectDateReturned()
		{
			CnbProvider provider = new CnbProvider();

			List<Currency> currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
			DateTime queryDateTime = new DateTime(2021, 3, 15);
			IEnumerable<ExchangeRate> rates = provider.GetExchangeRatesAsync(currencies, queryDateTime).Result;

			Assert.IsNotNull(rates);
			Assert.IsTrue(rates.All(p => p.DateTime.Equals(queryDateTime)));
		}

		[TestMethod()]
		public void GetExchangeRatesAsync_PastDate_CorrectExchangeRateReturned()
		{
			CnbProvider provider = new CnbProvider();

			List<Currency> currencies = new List<Currency> { new Currency("USD"), new Currency("EUR") };
			DateTime queryDateTime = new DateTime(2021, 3, 15);
			IEnumerable<ExchangeRate> rates = provider.GetExchangeRatesAsync(currencies, queryDateTime).Result;

			Assert.AreEqual(rates.First(p => p.SourceCurrency.Code.Equals("EUR")).Value,(decimal)26.195);
			Assert.AreEqual(rates.First(p => p.SourceCurrency.Code.Equals("USD")).Value,((decimal)21.974));
		}

		[TestMethod()]
		public void GetExchangeRatesAsync_CurrencyWithMultiplicator_PropertiesRead()
		{
			CnbProvider provider = new CnbProvider();

			List<Currency> currencies = new List<Currency> { new Currency("IDR"), new Currency("JPY"), new Currency("HUF") };
			DateTime queryDateTime = new DateTime(2021, 2, 15);
			IEnumerable<ExchangeRate> rates = provider.GetExchangeRatesAsync(currencies, queryDateTime).Result;

			var rate = rates.First(p => p.SourceCurrency.Code.Equals("IDR"));
			Assert.AreEqual(rate.TargetMultiplicator,1000);
			Assert.AreEqual(rate.SourceCurrency.Code,"IDR");
			Assert.AreEqual(rate.TargetCurrency.Code,"CZK");
			Assert.AreEqual(rate.DateTime,queryDateTime);
			Assert.AreEqual(rate.Value,(decimal)1.521);

			rate = rates.First(p => p.SourceCurrency.Code.Equals("JPY"));
			Assert.AreEqual(rate.TargetMultiplicator, 100);
			Assert.AreEqual(rate.SourceCurrency.Code, "JPY");
			Assert.AreEqual(rate.TargetCurrency.Code, "CZK");
			Assert.AreEqual(rate.DateTime, queryDateTime);
			Assert.AreEqual(rate.Value, (decimal)20.101);

			rate = rates.First(p => p.SourceCurrency.Code.Equals("HUF"));
			Assert.AreEqual(rate.TargetMultiplicator, 100);
			Assert.AreEqual(rate.SourceCurrency.Code, "HUF");
			Assert.AreEqual(rate.TargetCurrency.Code, "CZK");
			Assert.AreEqual(rate.DateTime, queryDateTime);
			Assert.AreEqual(rate.Value, (decimal)7.170);
		}
	}
}