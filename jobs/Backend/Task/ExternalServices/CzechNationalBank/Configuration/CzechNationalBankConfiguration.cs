namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration
{
    public class CzechNationalBankConfiguration
    {
        public string HttpProtocol { get; set; }
        public string DomainUrl { get; set; } 
        public CzechNationalBankApiClientEndpoints Endpoints { get; set; }
    }
}