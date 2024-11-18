namespace ExchangeRateUpdater.Domain.Shared
{
    public class Settings
    {
        public required string CnbUrl { get; set; }
        public required string DefaultCurrency { get; set; }
    }
}
