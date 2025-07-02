using System;

namespace ExchangeRateUpdater.Core.UseCases.CommonModels
{
	[Obsolete]
	public class Currency
	{
		/// <summary>
		/// Three-letter ISO 4217 code of the currency.
		/// </summary>
		public string Code { get; }

		public Currency(string code)
		{
			Code = code;
		}

		/// <inheritdoc/>
		public override string ToString()
		{
			return Code;
		}
	}
}
