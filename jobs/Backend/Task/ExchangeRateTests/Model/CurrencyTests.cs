using Common.Exceptions;
using Common.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Model.Tests
{
	[TestClass()]
	public class CurrencyTests
	{
		[TestMethod()]
		public void CurrencyClass_4CharsCurrencyString_ExceptionIsRaised()
		{
			try
			{
				Currency currency = new Currency("AAAA");
			}
			catch (IncorrectCurrencyCodeFormartException)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod()]
		public void CurrencyClass_2CharsCurrencyString_ExceptionIsRaised()
		{
			try
			{
				Currency currency = new Currency("AA");
			}
			catch (IncorrectCurrencyCodeFormartException)
			{
				return;
			}
			Assert.Fail();
		}

		[TestMethod()]
		public void CurrencyClass_3CharsCurrencyString_ExceptionIsNotRaised()
		{
			try
			{
				Currency currency = new Currency("AAA");
			}
			catch (IncorrectCurrencyCodeFormartException)
			{
				Assert.Fail();
			}
		}

		[TestMethod()]
		public void ToString_3CharsCurrencyString_CorrectFormat()
		{
			Currency currency = new Currency("CZK");
			Assert.AreEqual(currency.ToString(),"CZK");

		}

	}
}