using ExchangeRateUpdater.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Model;
using Common.Providers;
using ExchangeRateUpdater.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Providers.Tests
{
	[TestClass()]
	public class CnbParserTests
	{
		[TestMethod()]
		public void Validate_CorrectInput_ValidationSucceded()
		{
			string data = File.ReadAllText(@"TestData\validCnbData.txt");
			CnbParser parser = new CnbParser();
			Assert.IsTrue(parser.Validate(data));
		}

		[TestMethod()]
		public void Validate_InvalidDateRow_ValidationFails()
		{
			string data = File.ReadAllText(@"TestData\invalidDateCnbData.txt");
			CnbParser parser = new CnbParser();
			Assert.IsFalse(parser.Validate(data));
		}

		[TestMethod()]
		public void Validate_InvalidHeaderRow_ValidationFails()
		{
			string data = File.ReadAllText(@"TestData\invalidHeaderCnbData.txt");
			CnbParser parser = new CnbParser();
			Assert.IsFalse(parser.Validate(data));
		}

		[TestMethod()]
		public void Validate_ValidateEmptyFile_ValidationFails()
		{
			CnbParser parser = new CnbParser();
			Assert.IsFalse(parser.Validate(string.Empty));
		}

		[TestMethod()]
		public void Validate_ParseEmptyFile_ValidationFailsException()
		{
			try
			{
				CnbParser parser = new CnbParser();
				parser.Parse(string.Empty, new List<Currency>()).ToList();
			}
			catch (InvalidDataException)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod()]
		public void Validate_ParseValidFileWithNoCurrency_NoExchangeRatesInCollection()
		{
			string data = File.ReadAllText(@"TestData\validCnbData.txt");
			CnbParser parser = new CnbParser();
			IEnumerable<ExchangeRate>? exchangeRates = parser.Parse(data, new List<Currency>());
			Assert.IsFalse(exchangeRates.Any());
		}

		[TestMethod()]
		public void Validate_ParseValidFileWithUnsupportedCurrencies_NoExchangeRatesInCollection()
		{
			string data = File.ReadAllText(@"TestData\validCnbData.txt");
			CnbParser parser = new CnbParser();
			IEnumerable<ExchangeRate>? exchangeRates = parser.Parse(data, new List<Currency>() { new Currency("UNS"), new Currency("ABC"), new Currency("CZK") });
			Assert.IsFalse(exchangeRates.Any());
		}


		[TestMethod()]
		public void Validate_ParseValidFileWithSupportedCurrencies_NoExchangeRatesInCollection()
		{
			string data = File.ReadAllText(@"TestData\validCnbData.txt");
			CnbParser parser = new CnbParser();
			List<ExchangeRate> exchangeRates = parser.Parse(data, new List<Currency>() { new Currency("USD"), new Currency("RUB") }).ToList();
			
			Assert.AreEqual(exchangeRates.Count(), 2);
			Assert.IsTrue(exchangeRates.Any(p => p.SourceCurrency.Code.Equals("USD")));
			Assert.IsTrue(exchangeRates.Any(p => p.SourceCurrency.Code.Equals("RUB")));
		}
	}
}