using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class StringThrowUnitTest {
		[TestMethod]
		[ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
		public void IfNullOrWhiteSpace_NullParameter() {
			Throw.IfNullOrWhiteSpace(null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
		public void IfNullOrWhiteSpace_EmptyParameter() {
			Throw.IfNullOrWhiteSpace(String.Empty);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), AllowDerivedTypes = true)]
		public void IfNullOrWhiteSpace_WhiteSpaceParameter() {
			Throw.IfNullOrWhiteSpace(" ");
		}

		[TestMethod]
		public void IfNullOrWhiteSpace_NotNullOrWhiteSpaceParameter() {
			Throw.IfNullOrWhiteSpace("a");
		}
	}
}
