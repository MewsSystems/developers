using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Provider.Settings
{
    public class ExchangeRateProviderSettingsLoader
    {
        public static ProviderSettings Load(string configPath)
        {
            string? basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? 
                throw new InvalidOperationException("Failed to get the base path of the current assembly");

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile(configPath, optional: true, reloadOnChange: true);

            IConfigurationRoot configuration = configBuilder.Build();

            string? baseAddress = (configuration.GetSection("BaseAddress")?.Value) ??
                throw new InvalidOperationException("BaseAddress configuration is missing or null.");

            string? exchangeRateEndpoint = (configuration.GetSection("ExchangeRateEndpoint")?.Value) ??
                throw new InvalidOperationException("ExchangeRateEndpoint configuration is missing or null.");

            string? httpClientTimeoutString = (configuration.GetSection("HttpClientTimeout")?.Value) ??
                throw new InvalidOperationException("httpClientTimeout configuration is missing or null.");

            if (!int.TryParse(httpClientTimeoutString, out int httpClientTimeout))
            {
                throw new InvalidOperationException("Invalid value for httpClientTimeout configuration.");
            }

            return new ProviderSettings(baseAddress, exchangeRateEndpoint, httpClientTimeout);
        }
    }
}
