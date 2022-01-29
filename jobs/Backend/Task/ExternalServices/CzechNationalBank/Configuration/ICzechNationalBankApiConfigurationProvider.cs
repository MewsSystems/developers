namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration
{
    public interface ICzechNationalBankApiConfigurationProvider
    {
        CzechNationalBankConfiguration GetConfiguration();
    }
}