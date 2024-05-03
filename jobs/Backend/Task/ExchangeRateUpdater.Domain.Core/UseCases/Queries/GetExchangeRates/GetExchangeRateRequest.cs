using System;

namespace ExchangeRateUpdater.Domain.Core.UseCases.Queries.GetExchangeRates
{
	public class GetExchangeRateRequest
	{
		/// <summary>
		/// Target currency
		/// </summary>
		public string TargetCurrency { get; set; }

		/// <summary>
		/// Year-Month of the exchange value
		/// </summary>
		public DateTime? Date { get; set; }
	}
}
