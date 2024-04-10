using ExchangeRateUpdater.DependencyInjection;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Features;
using ExchangeRateUpdater.Infrastructure;
using ExchangeRateUpdater.Presentation;
using ExchangeRateUpdater.Settings;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ExchangeRates.DependencyInjection;

public static class HostBuilderExtensions
{
    public static IHostBuilder AddConfiguration(this IHostBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        builder
            .ConfigureAppConfiguration(x => x.AddConfiguration(configuration))
            .ConfigureServices((hostContext, services) =>
            {
                services.Configure<ExternalApis>(hostContext.Configuration.GetSection(nameof(ExternalApis)));
            });

        return builder;
    }

    public static IHostBuilder AddServices(this IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(HostBuilderExtensions).Assembly);
            });

            services.AddHttpClient();

            services.AddScoped<IValidator<GetExchangeRateOperation.Query>, GetExchangeRateOperation.RequestValidator>();

            services.AddScoped<IParser, Parser>();
            services.AddScoped<IExchangeRatesService, ExchangeRatesService>();
            services.AddScoped<GetExchangeRateOperation>();
            services.AddScoped<GetHelpOperation>();
        });

        return builder;
    }
}
