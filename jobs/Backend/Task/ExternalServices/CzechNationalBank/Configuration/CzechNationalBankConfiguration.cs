
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.ExternalServices.CzechNationalBank.Configuration
{
    public class CzechNationalBankConfiguration
    {
        public string HttpProtocol { get; set; }
        public string DomainUrl { get; set; } 
        public CzechNationalBankApiClientEndpoints Endpoints { get; set; }
        
        public static CzechNationalBankConfiguration GetCzechNationalBankConfiguration(IConfiguration configuration)
        {
            var czechNationalBankConfigurationName = nameof(CzechNationalBankConfiguration);
            var czechNationalBankConfiguration = new CzechNationalBankConfiguration
            {
                HttpProtocol = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.HttpProtocol)}").Value,
                DomainUrl = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.DomainUrl)}").Value,
                Endpoints = new CzechNationalBankApiClientEndpoints
                {
                    ExchangeRatePath = configuration.GetSection($"{czechNationalBankConfigurationName}:{nameof(CzechNationalBankConfiguration.Endpoints)}:{nameof(CzechNationalBankApiClientEndpoints.ExchangeRatePath)}").Value
                }
            };

            return czechNationalBankConfiguration;
        }
    }
}