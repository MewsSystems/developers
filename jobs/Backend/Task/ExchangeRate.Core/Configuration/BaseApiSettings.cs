using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Datalayer.Configuration
{
    public class BaseApiSettings : IBaseApiSettings
    {
        private readonly IConfiguration _configuration;
        private const string SettingsConfigurationKey = "ExchangeRateApiSettings";

        public BaseApiSettings(IConfiguration configuration)
        {
            _configuration = configuration;

            initialiseConfigSettings();
            
        }

        private void initialiseConfigSettings()
        {
            var apiSettingsSection = _configuration.GetSection(SettingsConfigurationKey);
            if (apiSettingsSection != null)
            {
                DailyFileUri = apiSettingsSection.GetSection("DailyFileUri") != null ? apiSettingsSection.GetSection("DailyFileUri").Value : "";
                SourceCurrency = apiSettingsSection.GetSection("SourceCurrency") !=null ? apiSettingsSection.GetSection("SourceCurrency").Value : "";
                CultureInfo = apiSettingsSection.GetSection("CultureInfo") != null ? apiSettingsSection.GetSection("CultureInfo").Value : "";
                int currencyIndex = -1;
                int.TryParse(apiSettingsSection.GetSection("CurrencyIndex") !=null ? apiSettingsSection.GetSection("CurrencyIndex").Value : "", out currencyIndex);
                CurrencyIndex = currencyIndex;
                int rateIndex = -1;
                int.TryParse(apiSettingsSection.GetSection("RateIndex") != null ? apiSettingsSection.GetSection("RateIndex").Value : "", out rateIndex);
                RateIndex = rateIndex;

            }

        }
        public string DailyFileUri { get; set; } = "";
        public string SourceCurrency { get; set; } = "";
        public string CultureInfo { get; set; } = "";
        public int CurrencyIndex { get; set; }
        public int RateIndex { get; set; }
    }
}
