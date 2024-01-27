using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using ExchangeRateUpdater.Services;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(
                (context, builder) =>
                {
                    builder
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", optional: false);
                })
                .ConfigureServices(
                (context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();

            host.Run();
        }

        private static void ConfigureServices(IConfiguration configuration, IServiceCollection services)
        {
            services.AddHostedService<ExchangeRateUpdaterJob>();
        }
    }
}
