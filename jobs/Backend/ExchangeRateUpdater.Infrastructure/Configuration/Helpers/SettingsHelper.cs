using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Infrastructure.Configuration.Helpers
{
    public static class SettingsHelper
    {
        private static IConfiguration _settings;

        public static void Initialize()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            _settings = builder.Build();
        }

        public static string GetCnbUrl() => _settings["cnbUrl"] ?? "https://api.cnb.cz/cnbapi/exrates/daily";
        public static string GetDefaultCurrency() => _settings["DefaultCurrency"] ?? "CZK";
    }
}
