using Microsoft.Extensions.Configuration;

namespace ExchangeRateUpdater.Features.Configuration
{
    public static class ConfigProvider
    {
        public static IConfiguration Configuration;

        public static IConfiguration GetConfig()
        {
            Configuration ??= new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                        .Build();

            return Configuration;
        }
    }

    public class AppSettingsProvider
    {
        private static IConfiguration Config => ConfigProvider.GetConfig();

        public static ExchangeRateUpdaterOptions CnbClientOptions => GetExchangeRateUpdaterOptions();


        private static ExchangeRateUpdaterOptions GetExchangeRateUpdaterOptions()
        {
            var result = new ExchangeRateUpdaterOptions();
            Config.GetSection("ExchangeRateUpdaterOptions").Bind(result);
            return result;
        }
    }
}
