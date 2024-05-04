﻿using System;

namespace ExchangeRateUpdater.Domain.Core.UseCases.CommonModels
{
	public class ExchangeRate
	{
		public Currency SourceCurrency { get; }

		public Currency TargetCurrency { get; }

		public DateTime ValidFor { get; }

		public decimal Value { get; }

		public ExchangeRate(Currency sourceCurrency, Currency targetCurrency, DateTime validFor, decimal value)
		{
			SourceCurrency = sourceCurrency;
			TargetCurrency = targetCurrency;
			ValidFor = validFor;
			Value = value;
		}

		public override string ToString()
		{
			return $"{SourceCurrency}/{TargetCurrency}={Value}";
		}
	}
}
