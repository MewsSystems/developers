using Autofac;
using Mews.CurrencyExchange.Providers.Abstractions;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client;
using Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Provider;
using Microsoft.Extensions.Configuration;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mews.CurrencyExchange.Providers.CzechNationalBank.Tests")]
namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Configuration
{
    public class CzechNationalBankProviderModule : Module
    {
        private const string ConnectorSettingsConfigurationKey = "ConnectorSettings";

        private readonly IConfiguration _configuration;

        public CzechNationalBankProviderModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CurrencyExchangeProvider>()
                   .As<ICurrencyExchangeProvider>();

            var connectorSettings = _configuration.GetSection(ConnectorSettingsConfigurationKey).Get<ConnectorSettings>();
            builder.RegisterInstance(connectorSettings);

            builder.RegisterType<CzechNationalBankExchangeClient>()
                   .As<ICzechNationalBankExchangeClient>();
        }
    }
}
