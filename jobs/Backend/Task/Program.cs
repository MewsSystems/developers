using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using ExchangeRateUpdater.Configuration;
using ExchangeRateUpdater.Clients;
using ExchangeRateUpdater.Services;
using ExchangeRateUpdater.Parsers;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
             .ConfigureServices((hostContext, services) =>
             {
                 services.AddHostedService<Startup>();
                 services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>()
                         .AddSingleton<ICzechNationalBankConfiguration, CzechNationalBankConfiguration>()
                         .AddSingleton<ICzechNationalBankClient, CzechNationalBankClient>()
                         .AddSingleton<ICNBExchangeRateParser,CNBExchangeRateParser>()
                         .AddSingleton<IExchangeRateService, ExchangeRateService>();                 
              });
        }
    }
}
   