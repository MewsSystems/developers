using ExchangeRate.Domain;

namespace ExchangeRate.Service.Service
{
	/// <summary>
	/// Base exchange rate service for concrete implementation
	/// </summary>
	public class BaseExchangeRateService
	{
		/// <summary>
		/// Create new exchange rare domain model
		/// </summary>
		/// <param name="sourceCurrencyCode">Source currency code</param>
		/// <param name="rate">exchange rate</param>
		/// <param name="amount">exchange amount</param>
		/// <param name="targetCurrencyCode">target currency code</param>
		/// <returns></returns>
		protected static Domain.ExchangeRate CreateNewExchangeRate(string? sourceCurrencyCode, decimal rate, int amount, string targetCurrencyCode)
			=> new(new Currency(sourceCurrencyCode), new Currency(targetCurrencyCode), rate / amount);
	}
}
