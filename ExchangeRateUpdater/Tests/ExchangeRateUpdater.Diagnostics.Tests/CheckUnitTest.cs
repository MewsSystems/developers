namespace ExchangeRateUpdater.Diagnostics.Tests {
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;

	[TestClass]
	public class CheckUnitTest {
		[TestMethod]
		public void IsNull_NullParameter() {
			var result = Check.IsNull(null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNull_NotNullParameter() {
			var result = Check.IsNull(default(int));

			Assert.IsFalse(result);
		}
	}
}
