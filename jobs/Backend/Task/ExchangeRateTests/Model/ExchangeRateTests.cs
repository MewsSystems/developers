using System;
using Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Model.Tests
{
	[TestClass()]
	public class ExchangeRateTests
	{
		[TestMethod()]
		public void ExchangeRateClass_CzkCurrencyOnInput_CzkNotInResult()
		{
			DateTime time = new DateTime(2022, 5, 6);
			ExchangeRate rate = new ExchangeRate(new("EUR"), new Currency("CZK"), (decimal)15.20, time, 100);

			Assert.AreEqual(rate.TargetMultiplicator, 100);
			Assert.AreEqual(rate.SourceCurrency.Code, "EUR");
			Assert.AreEqual(rate.TargetCurrency.Code, "CZK");
			Assert.AreEqual(rate.DateTime, time);
			Assert.AreEqual(rate.Value, (decimal)15.20);
		}

		[TestMethod()]
		public void ExchangeRateClass_Multiplication_RestultWithMultilication()
		{
			DateTime time = new DateTime(2022, 5, 6);
			ExchangeRate rate = new ExchangeRate(new("RUB"), new Currency("CZK"), (decimal)15.20, time, 100);

			Assert.AreEqual(rate.TargetMultiplicator, 100);
			Assert.AreEqual(rate.SourceCurrency.Code, "RUB");
			Assert.AreEqual(rate.TargetCurrency.Code, "CZK");
			Assert.AreEqual(rate.DateTime, time);
			Assert.AreEqual(rate.Value, (decimal)15.20);
			Assert.AreEqual(rate.MultipliedValue, (decimal)0.1520);
			Assert.AreEqual(rate.ToString(), $"{rate.SourceCurrency}/{rate.TargetCurrency}={rate.MultipliedValue}");
		}

		[TestMethod()]
		public void ExchangeRateClass_NoMultiplication_RestultWithoutMultilication()
		{
			DateTime time = new DateTime(2022, 5, 6);
			ExchangeRate rate = new ExchangeRate(new("USD"), new Currency("CZK"), (decimal)15.20, time);

			Assert.AreEqual(rate.TargetMultiplicator, 1);
			Assert.AreEqual(rate.SourceCurrency.Code, "USD");
			Assert.AreEqual(rate.TargetCurrency.Code, "CZK");
			Assert.AreEqual(rate.DateTime, time);
			Assert.AreEqual(rate.Value, (decimal)15.20);
			Assert.AreEqual(rate.MultipliedValue, (decimal)15.20);
			Assert.AreEqual(rate.ToString(), $"{rate.SourceCurrency}/{rate.TargetCurrency}={rate.MultipliedValue}");
		}
	}
}