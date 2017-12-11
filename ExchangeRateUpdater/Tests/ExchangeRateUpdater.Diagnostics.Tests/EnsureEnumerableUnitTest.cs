using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class EnsureEnumerableUnitTest {
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNotNullOrEmpty_NullParameter() {
			var expected = (IEnumerable<object>)null;

			var actual = Ensure.IsNotNullOrEmpty(expected);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void IsNotNullOrEmpty_EmptyParameter() {
			var expected = Enumerable.Empty<object>();

			var actual = Ensure.IsNotNullOrEmpty(expected);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsNotNullOrEmpty_NotNullOrEmptyParameter() {
			var expected = new object[] { default(object) };

			var actual = Ensure.IsNotNullOrEmpty(expected);

			Assert.AreEqual(expected, actual);
		}
	}
}
