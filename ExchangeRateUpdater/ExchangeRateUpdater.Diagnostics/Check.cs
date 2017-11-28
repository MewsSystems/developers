namespace ExchangeRateUpdater.Diagnostics {
	public partial class Check {
		public static bool IsNull(object value) {
			return value is null;
		}

		public static bool IsNotNull(object value) {
			return !IsNull(value);
		}
	}
}
