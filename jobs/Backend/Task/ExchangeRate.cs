using System;

namespace ExchangeRateUpdater
{
	public class ExchangeRate
	{
		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateTime lastUpdate)
		{
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			Value = value;
			LastUpdate = lastUpdate;
		}

		public Currency SourceCurrency { get; }

		public Currency TargetCurrency { get; }

		public decimal Value { get; }
		
		public DateTime LastUpdate { get; }

		public override string ToString()
		{
			return $"{SourceCurrency}/{TargetCurrency}={Value}";
		}
	}
}
