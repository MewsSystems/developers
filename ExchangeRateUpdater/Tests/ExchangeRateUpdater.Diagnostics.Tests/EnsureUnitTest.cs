using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class EnsureUnitTest {
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
		public void IsNotNull_NullParameter() {
			var expected = (object)null;

			var actual = Ensure.IsNotNull(expected);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsNotNull_NotNullParameter() {
			var expected = default(int);

			var actual = Ensure.IsNotNull(expected);

			Assert.AreEqual(expected, actual);
		}
	}
}
