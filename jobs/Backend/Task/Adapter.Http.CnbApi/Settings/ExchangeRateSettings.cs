namespace Adapter.Http.CnbApi.Settings
{
    public class ExchangeRateSettings
    {
        public required string ApiUrl { get; set; }
        public required string DefaultCurrency { get; set; }
        public required string[] Currencies { get; set; }
    }
}
