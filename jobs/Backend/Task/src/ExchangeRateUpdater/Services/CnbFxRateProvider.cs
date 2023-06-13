using ExchangeRateUpdater.Contracts;

namespace ExchangeRateUpdater.Services
{
	/// <summary>
	/// Provides FX rates from the CNB daily FX rates.
	/// </summary>
	public class CnbFxRateProvider : ICnbFxRateProvider
	{
		private readonly ICnbClient client;
		private readonly ICnbParser parser;

		public CnbFxRateProvider(ICnbClient client, ICnbParser parser)
		{
			this.client = client;
			this.parser = parser;
		}
		
		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>
		/// <param name="currencies">Currencies to get FX rates for.</param>
		/// <param name="date">Date to get rates or <c>null</c> for the latest rate. This semantics reflects the CNB API.</param>
		/// <remarks>
		/// The usage of CNB API semantics regarding the date parameter is a simplification.
		/// In case of multiple services, it might be better to provide today explicitly via some IDateTimeProvider.
		/// That way the behaviour of multiple services would be more consistent.
		/// </remarks>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateOnly? date = null)
		{
			string data = await client.GetRatesAsync(date);
			var currencyHashSet = new HashSet<Currency>(currencies);
			var parsedData = parser.Parse(data);
			return parsedData.Where(rate => currencyHashSet.Contains(rate.SourceCurrency));
		}
	}
}