using ExchangeRateUpdater.Core.Configuration;
using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Console
{
    public class Settings
    {
        public Settings(IConfiguration configuration)
        {
            configuration.Bind(this);
        }

        public CzechNationalBankConfiguration CzechNationalBankConfiguration { get; set; }

        public AppConfiguration AppConfiguration { get; set; }
    }
}
