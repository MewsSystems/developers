using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class StringCheckUnitTest {
		[TestMethod]
		public void IsNullOrEmpty_NullParameter() {
			var result = Check.IsNullOrEmpty((string)null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrEmpty_EmptyParameter() {
			var result = Check.IsNullOrEmpty(String.Empty);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrEmpty_NotNullOrEmptyParameter() {
			var result = Check.IsNullOrEmpty("a");

			Assert.IsFalse(result);
		}

		[TestMethod]
		public void IsNullOrWhiteSpace_NullParameter() {
			var result = Check.IsNullOrWhiteSpace((string)null);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrWhiteSpace_EmptyParameter() {
			var result = Check.IsNullOrWhiteSpace(String.Empty);

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrWhiteSpace_WhiteSpaceParameter() {
			var result = Check.IsNullOrWhiteSpace(" ");

			Assert.IsTrue(result);
		}

		[TestMethod]
		public void IsNullOrWhiteSpace_NotNullOrWhiteSpaceParameter() {
			var result = Check.IsNullOrEmpty("a");

			Assert.IsFalse(result);
		}
	}
}
