namespace ExchangeRateUpdater.Diagnostics {
	public partial class Ensure {
		public static string IsNotNullOrWhiteSpace(string value, string paramName = null) {
			Throw.IfNullOrWhiteSpace(value, paramName ?? nameof(value));

			return value;
		}
	}
}
