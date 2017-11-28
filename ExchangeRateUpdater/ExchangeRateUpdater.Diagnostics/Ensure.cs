namespace ExchangeRateUpdater.Diagnostics {
	public partial class Ensure {
		public static T IsNotNull<T>(T value, string paramName = null) {
			Throw.IfNull(value, paramName);

			return value;
		}
	}
}
