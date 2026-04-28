using Microsoft.Extensions.Configuration;
using System;

namespace ExchangeRateUpdater
{
    public class ConfigurationReader(IConfiguration config) : IConfigurationReader
    {
        private readonly IConfiguration _config = config;
        private const string SectionName = "CnbSettings";

        public CnbSettings ReadSettings()
        {
            var section = _config.GetSection("CnbSettings");
            string url = section["SourceUrl"] ?? throw new InvalidOperationException($"{SectionName}:SourceUrl is missing in appsettings.json.");
            string target = section["TargetCurrencyCode"] ?? "CZK";

            if (!int.TryParse(section["RetryAttempts"], out int retryAttempts))
            {
                retryAttempts = 3; // Default fallback
            }

            return new CnbSettings(url, target, retryAttempts);
        }
    }
}

