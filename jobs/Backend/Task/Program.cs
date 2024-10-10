using ExchangeRateUpdater.CzechNationalBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater;

internal partial class Program
{
    static void Main(string[] args)
    {
        using ILoggerFactory factory = LoggerFactory.Create(builder => builder.AddConsole());
        ILogger logger = factory.CreateLogger(nameof(Program));

        try
        {
            IHostBuilder builder = Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    services.Configure<HostedServiceConfiguration>(context.Configuration.GetSection("ExchangeRateUpdater"));

                    services.AddLogging(builder =>
                    {
                        builder.AddSimpleConsole(options => options.SingleLine = true);
                    });

                    services.AddCzechNationalBankClient(context.Configuration);
                    services.AddScoped<IExchangeRateProvider, ExchangeRateProvider>();
                    services.AddHostedService<HostedService>();
                });

            IHost host = builder.Build();
            host.Run();
        }
        catch (Exception ex)
        {
            LogFatal(logger, ex.Message);
        }
    }

    [LoggerMessage(Level = LogLevel.Critical, Message = "Start up failed. {errorMessage}.")]
    static partial void LogFatal(ILogger logger, string errorMessage);
}