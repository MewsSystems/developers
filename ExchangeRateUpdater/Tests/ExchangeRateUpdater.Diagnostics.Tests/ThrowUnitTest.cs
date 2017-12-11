using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExchangeRateUpdater.Diagnostics.Tests {
	[TestClass]
	public class ThrowUnitTest {
		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), AllowDerivedTypes = true)]
		public void ThrowIfNull_NullParameter() {
			Throw.IfNull<object>(null);
		}

		[TestMethod]
		public void ThrowIfNull_NotNullParameter() {
			Throw.IfNull<object>(default(int));
		}
	}
}
