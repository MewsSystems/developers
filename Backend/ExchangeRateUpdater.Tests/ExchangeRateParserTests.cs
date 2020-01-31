using System.Linq;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Tests
{
	[TestClass]
	public class ExchangeRateParserTests
	{
		[TestMethod]
		public void Parse_SingleCurrency_CorrectCurrencyRate()
		{
			var parser = new ExchangeRateParser("CZK", "cs-CZ", '\n', ';', 0, 2, 1, 0,"a;b;c");

			string input = "a;b;c\n" +
						   "USD;1;23,05;";

			var result = parser.Parse(input);

			var expected = new ExchangeRate(new Currency("CZK"), new Currency("USD"), 23.05m);

			Assert.AreEqual(expected.ToString(), result.Single().ToString());
		}

		[TestMethod]
		public void Parse_DifferentHeaderFormat_IncorrectCsvFormatException()
		{
			var parser = new ExchangeRateParser("CZK", "cs-CZ", '\n', ';', 0, 2, 1, 0, "a;b;c");

			string input = "x;y;z";

			Assert.ThrowsException<IncorrectCsvFormatException>(() => parser.Parse(input));
		}
	}
}
