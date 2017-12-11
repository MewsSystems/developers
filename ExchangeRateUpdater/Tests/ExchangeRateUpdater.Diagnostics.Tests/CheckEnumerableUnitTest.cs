using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class CheckEnumerableUnitTest {
		[TestMethod]
		public void IsNullOrEmpty_NullParameter() {
			var result = Check.IsNullOrEmpty((IEnumerable)null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrEmpty_EmptyParameter() {
			var result = Check.IsNullOrEmpty(Enumerable.Empty<object>());

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrEmpty_NotNullOrEmptyParameter() {
			var result = Check.IsNullOrEmpty("A".ToCharArray());

			Assert.IsFalse(result);
		}
	}
}
