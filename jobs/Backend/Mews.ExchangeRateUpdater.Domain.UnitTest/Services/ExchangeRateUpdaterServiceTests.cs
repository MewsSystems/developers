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
    public class ExchangeRateUpdaterServiceTests
    {
        [Fact]
        public async Task GetExchangeRates_Returns_Empty_Result_When_CurrencyCodes_Is_Empty()
        {
            var service = GetExchangeRateUpdaterService();

            var result = await service.GetExchangeRates(Enumerable.Empty<string>(), DateTime.Now);

            Assert.True(!result.Any());
        }

        private static IExchangeRateUpdaterService GetExchangeRateUpdaterService()
        {
            var services = GetServices();

            return services.GetService<IExchangeRateUpdaterService>();
        }

        private static IServiceProvider GetServices()
        {
            return new ServiceCollection()
                .AddHttpClient()
                .AddSingleton<CNB.CnbClient>()
                .AddTransient<IExchangeRateProvider, CNBExchangeRateProvider>()
                .AddTransient<IExchangeRateUpdaterService, ExchangeRateUpdaterService>()
                .AddSingleton(config => new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile(new EntityProfile());
                }).CreateMapper())
                .BuildServiceProvider();
        }
    }
}
