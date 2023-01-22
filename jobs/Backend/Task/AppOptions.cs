namespace ExchangeRateUpdater
{
	public class AppOptions
	{
		public IEnumerable<string> CurrencyCodes { get; set; } = new List<string>();
	}
}