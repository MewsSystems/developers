using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Financial.Tests {
	[TestClass]
	public class CurrencyUnitTest {
		[TestMethod]
		public void Currency_NullParameter() {
			var ex = Assert.ThrowsException<ArgumentException>(() => new Currency(null));

			Assert.AreEqual(ex.ParamName, "code");
		}

		[TestMethod]
		public void Currency_EmptyParameter() {
			var ex = Assert.ThrowsException<ArgumentException>(() => new Currency(String.Empty));

			Assert.AreEqual(ex.ParamName, "code");
		}

		[TestMethod]
		public void Currency_ValidParameter() {
			var code = "a";

			var value = new Currency(code);

			Assert.AreEqual(code, value.Code);
		}

		[TestMethod]
		public void Currency_ValidParameter_ToString() {
			var code = "a";

			var value = new Currency(code);

			Assert.AreEqual(code, value.ToString());
		}
	}
}
