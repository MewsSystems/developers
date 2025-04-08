namespace ExchangeRateUpdater
{
	public class ExchangeRate
	{
		public ExchangeRate(Currency sourceCurrency, int sourceAmount, decimal targetRate){
			this.sourceCurrency = sourceCurrency;
			this.sourceAmount = sourceAmount;
			this.targetCurrency = new Currency("CZK");
			this.targetRate = targetRate;
		}

		public Currency sourceCurrency { get; }

		public Currency targetCurrency { get; }

		public int sourceAmount { get; }

		public decimal targetRate { get; }

		public override string ToString() {
			return $"{sourceCurrency}/{targetCurrency} = {sourceAmount}/{targetRate}";
		}
	}
}
