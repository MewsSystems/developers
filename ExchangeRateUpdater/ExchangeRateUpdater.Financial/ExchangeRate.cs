namespace ExchangeRateUpdater.Financial {
	using ExchangeRateUpdater.Diagnostics;

	public struct ExchangeRate {
		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value) {
			SourceCurrency = Ensure.IsNotNull(sourceCurrency, nameof(sourceCurrency));
			TargetCurrency = Ensure.IsNotNull(targetCurrency, nameof(targetCurrency));
			Value = value;
		}

		public Currency SourceCurrency { get; private set; }

		public Currency TargetCurrency { get; private set; }

		public decimal Value { get; private set; }

		public override string ToString() {
			return $"{SourceCurrency.Code} / {TargetCurrency.Code} = {Value}";
		}
	}
}
