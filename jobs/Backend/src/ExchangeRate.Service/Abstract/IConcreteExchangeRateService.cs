namespace ExchangeRate.Service.Abstract
{
	/// <summary>
	///     Exchange rate service for CNB APIs TXT, XML
	/// </summary>
	public interface IConcreteExchangeRateService
	{
		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		/// <param name="targetCurrencyCode">Target currency code</param>
		/// <returns>list string exchange rates from concrete api</returns>
		Task<List<string>?> GetExchangeRates(string targetCurrencyCode);
	}
}
