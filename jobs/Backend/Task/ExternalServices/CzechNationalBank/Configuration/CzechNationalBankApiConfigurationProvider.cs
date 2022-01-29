namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration
{
    public class CzechNationalBankApiConfigurationProvider : ICzechNationalBankApiConfigurationProvider
    {
        private readonly CzechNationalBankConfiguration _configuration;

        public CzechNationalBankApiConfigurationProvider(CzechNationalBankConfiguration configuration)
        {
            _configuration = configuration;
        }

        public CzechNationalBankConfiguration GetConfiguration() => _configuration;
    }
}