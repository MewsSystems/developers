using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Configurations
{
    public class CzechNationalBankConfig : ICzechNationalBankConfig
    {
        private readonly IConfiguration configuration;

        public CzechNationalBankConfig(IConfiguration configuration)
        {
            this.configuration = configuration.GetSection("CzechNationalBank");
        }

        public string ExchangeRateUrl { get => configuration.GetValue<string>("ExchangeRateUrl"); }
    }
}
