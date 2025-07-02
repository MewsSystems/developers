using System;

namespace ExchangeRateUpdater.Core.UseCases.CommonModels
{
	public class ExchangeRate
	{
		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string SourceCurrency { get; }

		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string TargetCurrency { get; }

		public DateTime ValidFor { get; }

		public decimal Value { get; }

		public ExchangeRate(string sourceCurrency, string targetCurrency, DateTime validFor, decimal value)
		{
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			ValidFor = validFor;
			Value = value;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"{SourceCurrency}/{TargetCurrency}={Value}";
		}
	}
}
