using Autofac;
using ExchangeRate.Datalayer.Configuration;
using ExchangeRate.Datalayer.Services;
using ExchangeRates.Providers.CzechNationalBank.Provider;
using ExchangeRates.Providers.CzechNationalBank.Service;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Providers.CzechNationalBank.AutofacConfiguration
{
    public class CzechNationalBankModule : Autofac.Module
    {
        private const string SettingsConfigurationKey = "ExchangeRateApiSettings";

        private readonly IConfiguration _configuration;

        public CzechNationalBankModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {

            var connectorSettings = _configuration.GetSection(SettingsConfigurationKey);
            builder.RegisterInstance(connectorSettings);


            builder.RegisterType<CurrencyExchangeProvider>()
                   .As<ICurrencyExchangeProvider>();
            
            

            builder.RegisterType<BaseApiSettings>()
                   .As<IBaseApiSettings>();


            builder.RegisterType<CzechNationalBankExchangeService>()
                   .As<IExchangeRateService>();
        }
    }
}
