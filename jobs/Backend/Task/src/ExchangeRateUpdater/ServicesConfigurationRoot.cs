using System;
using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater
{
    public static class ServicesConfigurationRoot
    {
        public static IServiceProvider ServiceProvider { get; } = BuildServiceProvider();

        public static IServiceProvider BuildServiceProvider()
        {
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("applicationSettings.json").Build();
            return new ServiceCollection()
                .AddInfrastructureLayer(configuration)
                .AddApplicationLayer()
                .AddLogging(configure => configure.AddConsole())                    
                .Configure<LoggerFilterOptions>(configuration)                    
                .BuildServiceProvider();
        }
    }
}