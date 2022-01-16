using AutoMapper;
using Mews.ExchangeRateUpdater.Domain.AutoMapper;
using Mews.ExchangeRateUpdater.Domain.Interfaces;
using Mews.ExchangeRateUpdater.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Mews.ExchangeRateUpdater.Domain.UnitTest.Providers
{
    public class CNBExchangeRateProviderTests
    {
        [Fact]
        public async Task GetExchangeRates_Returns_Empty_Result_When_CurrencyCodes_Is_Empty()
        {
            var provider = GetExchangeRateProvider();

            var result = await provider.GetExchangeRates(Enumerable.Empty<string>(), DateTime.Now);

            Assert.True(!result.Any());
        }

        private static IExchangeRateProvider GetExchangeRateProvider()
        {
            var services = GetServices();

            return services.GetService<IExchangeRateProvider>();
        }

        private static IServiceProvider GetServices()
        {
            return new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<CNB.CnbClient>()
                .AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>()
                .AddSingleton(config => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new EntityProfile());
                }).CreateMapper())
                .BuildServiceProvider();
        }
    }
}
