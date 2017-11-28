using System;

namespace ExchangeRateUpdater.Diagnostics {
	public partial class Throw {
		public static void IfNullOrWhiteSpace(string value, string paramName = null) {
			if (Check.IsNullOrWhiteSpace(value)) {
				Throw.StringEmptyOrWhiteSpaceArgumentException(paramName ?? nameof(value));
			}
		}
	}
}
