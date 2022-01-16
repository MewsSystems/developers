using AutoMapper;
using Mews.ExchangeRateUpdater.Domain.AutoMapper;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExchangeRateUpdater
{
    public static class HostBuilder
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services
                    .AddHttpClient()
                    .AddSingleton<CNB.CnbClient>()
                    .AddTransient<IExchangeRateUpdaterService, ExchangeRateUpdaterService>()
                    .AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>()
                    .AddSingleton(config => new MapperConfiguration(cfg =>
                    {
                        cfg.AddProfile(new EntityProfile());
                    }).CreateMapper()));
        }
    }
}
