using ExchangeRate.Domain;

namespace ExchangeRate.Service.Models
{
	/// <summary>
	/// Currency constants
	/// </summary>
	public static class CurrencyConstants
	{
		/// <summary>
		/// Allowed currencies
		/// </summary>
		public static readonly IEnumerable<Currency> AllowedCurrencies = new List<Currency>
		{
			new ("USD"),
			new ("EUR"),
			new ("CZK"),
			new ("JPY"),
			new ("KES"),
			new ("RUB"),
			new ("THB"),
			new ("TRY"),
			new ("XYZ")
		};
	}
}
