namespace ExchangeRateUpdater.Contracts
{
	public class AppSettings
	{
		public CnbDailyRatesOptions CnbDaily { get; set; } = new();
	}
}