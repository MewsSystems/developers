namespace OpenExchangeRates {
	using ExchangeRateUpdater.Diagnostics;

	public struct AppId {
		public AppId(string value) {
			Value = Ensure.IsNotNullOrWhiteSpace(value, nameof(value));
		}

		public string Value { get; private set; }

		public override int GetHashCode() {
			return Value.GetHashCode();
		}

		public override string ToString() {
			return Value;
		}
	}
}
