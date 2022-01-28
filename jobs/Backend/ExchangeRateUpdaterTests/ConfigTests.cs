using ExchangeRateUpdater;
using ExchangeRateUpdater.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExchangeRateUpdaterTests
{
    public class ConfigTests
    {
        [Fact]
        public void DependecyInjection_CanResolveExchangeRateProvider()
        {
            var host = Program.CreateHostBuilder(Array.Empty<string>()).Build();
            var provider = host.Services.GetRequiredService<IExchangeRateProvider>();
            Assert.NotNull(provider);
        }

        [Fact]
        public void CzechNationalBankConfig_ReadsFromConfiguration()
        {
            var host = Program.CreateHostBuilder(Array.Empty<string>())
                .ConfigureAppConfiguration(((host, conf) =>
                {
                    conf
                    .AddInMemoryCollection(new Dictionary<string, string>()
                    {
                        { "CzechNationalBank:ExchangeRateUrl", "http://localhost" },
                    });
                })).Build();

            var config = host.Services.GetRequiredService<ICzechNationalBankConfig>();
            Assert.Equal("http://localhost", config.ExchangeRateUrl);
        }
    }
}
