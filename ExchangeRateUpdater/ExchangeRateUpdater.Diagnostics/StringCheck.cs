namespace ExchangeRateUpdater.Diagnostics {

	using System;

	public partial class Check {
		public static bool IsNullOrEmpty(string value) {
			return String.IsNullOrEmpty(value);
		}

		public static bool IsNullOrWhiteSpace(string value) {
			return String.IsNullOrWhiteSpace(value);
		}
	}
}
