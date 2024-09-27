using ExchangeRateUpdater.Application;
using ExchangeRateUpdater.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly.Extensions.Http;
using Polly;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using FluentValidation;

namespace ExchangeRateUpdater
{
    public static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            using var host = BuildHost(args);

            var currencies = ReadCurrenciesFromConfiguration(host);
            var provider = host.Services.GetRequiredService<ExchangeRateProvider>();

            using var loggerFactory = host.Services.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(nameof(Program));

            try
            {
                var rates = await provider.GetExchangeRatesAsync(currencies, CancellationToken.None);

                logger.LogInformation("Successfully retrieved {count} exchange rates:\n{rates}",
                    rates.Count(), string.Join('\n', rates));
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not retrieve exchange rates");
                return 1;
            }

            Console.ReadLine();
            return 0;
        }

        private static IHost BuildHost(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddHttpClient<ICzechNationalBankClient, CzechNationalBankClient>(client =>
                        {
                            client.BaseAddress = new Uri(context.Configuration["ExchangeRateApiBaseAddress"]);
                        })
                        .AddPolicyHandler(GetRetryPolicy());

                    services.AddTransient<ExchangeRateProvider>();
                    services.AddValidatorsFromAssemblyContaining<CnbExchangeRateValidator>();
                })
                .Build();
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(4, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IEnumerable<Currency> ReadCurrenciesFromConfiguration(IHost hostBuilder)
        {
            var configuration = hostBuilder.Services.GetRequiredService<IConfiguration>();

            return configuration
                .GetSection("Currencies")
                .Get<string[]>()
                .Select(code => new Currency(code));
        }
    }
}
