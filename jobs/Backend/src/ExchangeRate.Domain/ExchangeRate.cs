namespace ExchangeRate.Domain
{
	public class ExchangeRate
	{
		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
		{
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			Value = value;
		}

		private Currency SourceCurrency { get; }

		private Currency TargetCurrency { get; }

		private decimal Value { get; }

		public override string ToString() => $"{SourceCurrency}/{TargetCurrency}={Value}";
	}
}
